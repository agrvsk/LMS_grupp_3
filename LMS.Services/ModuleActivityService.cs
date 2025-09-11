using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.AspNetCore.Http;
using Service.Contracts;
using Domain.Models.Exceptions;

namespace LMS.Services;

public class ModuleActivityService : IModuleActivityService
{
    private readonly IUnitOfWork uow;
    private readonly IMapper mapper;
    private readonly IFileHandlerService fileHandlerService;

    public ModuleActivityService(IUnitOfWork uow, IMapper mapper, IFileHandlerService fileHandlerService)
    {
        this.uow = uow;
        this.mapper = mapper;
        this.fileHandlerService = fileHandlerService;
    }


    public async Task<ModuleActivityDto?> GetModuleActivityByIdAsync(Guid moduleActivityId)
    {
        var moduleActivity = await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivityId);
        if(moduleActivity == null) throw new ActivityNotFoundException(moduleActivityId);
        var moduleActivityDto = mapper.Map<ModuleActivityDto>(moduleActivity);

        return moduleActivityDto;
    }
    public async Task<List<ModuleActivityDto>> GetAllModuleActivitiesAsync()
    {
        var moduleActivities = await uow.ModuleActivityRepository.GetAllModuleActivitiesAsync();
        var moduleActivityDtos = mapper.Map<List<ModuleActivityDto>>(moduleActivities);
        return moduleActivityDtos.OrderBy(c => c.StartDate).ToList();
    }
    public async Task<List<ModuleActivityDto>> GetModuleActivitiesByModuleIdAsync(Guid moduleId)
    {
        var moduleActivities = await uow.ModuleActivityRepository.GetModuleActivitiesByModuleIdAsync(moduleId);
        var moduleActivityDtos = mapper.Map<List<ModuleActivityDto>>(moduleActivities);
        return moduleActivityDtos.OrderBy(c => c.StartDate).ToList();
    }
    public async Task<ModuleActivity> CreateActivityAsync(ModuleActivityCreateDto moduleActivity)
    {
        var activity = mapper.Map<ModuleActivity>(moduleActivity);
        uow.ModuleActivityRepository.Create(activity);
        await uow.CompleteAsync();
        return activity;
    }
    public async Task<ModuleActivityDto> CreateActivityWithDocumentsAsync(ModuleActivityCreateDto activityDto, List<IFormFile> files)
    {
        // 1. Map the activity
        var activity = mapper.Map<ModuleActivity>(activityDto);
        activity.Id = Guid.NewGuid();

        uow.ModuleActivityRepository.Create(activity);

        // Prepare a lookup for uploaded files by TempId
        var fileLookup = files.ToDictionary(f => f.Name, f => f);

        // 2. Handle assignments and documents
        foreach (var assignmentDto in activityDto.Assignments)
        {
            var assignment = mapper.Map<Assignment>(assignmentDto);
            assignment.Id = Guid.NewGuid();

            assignment.ModuleActivityId = activity.Id;
            assignment.Submissions = new List<Submission>();
            uow.AssignmentRepository.Create(assignment);

            foreach (var docMeta in assignmentDto.Documents)
            {
                if (!fileLookup.TryGetValue(docMeta.TempId, out var file))
                    throw new Exception($"File for TempId '{docMeta.TempId}' not found.");
                
                // Save file to disk
                var extension = Path.GetExtension(file.FileName);
                var safeName = Path.GetFileNameWithoutExtension(file.FileName);
                var docId = Guid.NewGuid();
                var fileName = $"{safeName}_{docId}{extension}";

                using var stream = file.OpenReadStream();
                var filePath = await fileHandlerService.UploadFileAsync(
                    stream,
                    fileName,
                    $"Uploads/Assignments"
                );

                // Create document entity
                var document = new Document
                {
                    Id = docId,
                    Name = docMeta.Name,
                    Description = docMeta.Description,
                    UploadDate = DateTime.UtcNow,
                    FilePath = filePath,
                    ParentType = "Activity",
                    ParentId = activity.Id,   // <--- activity link
                    UploaderId = docMeta.UploaderId!,
                    FileType = docMeta.FileType
                };

                uow.DocumentRepository.Create(document);
            }
        }

        await uow.CompleteAsync();

        return mapper.Map<ModuleActivityDto>(activity);
    }
    public async Task<ModuleActivityDto> UpdateActivityAsync(ModuleActivityUpdateDto moduleActivity)
    {
        var existingActivity = await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivity.Id);
        if (existingActivity == null)
            return null;
        mapper.Map(moduleActivity, existingActivity);
        uow.ModuleActivityRepository.Update(existingActivity);
        await uow.CompleteAsync();
        return (mapper.Map<ModuleActivityDto>(existingActivity));
    }
    public async Task<bool> DeleteActivityAsync(Guid moduleActivityId)
    {
        var activity = await uow.ModuleActivityRepository.GetModuleActivityByIdAsync(moduleActivityId);
        if (activity == null)
            return false;
        uow.ModuleActivityRepository.Delete(activity);
        await uow.CompleteAsync();
        return true;
    }

}
