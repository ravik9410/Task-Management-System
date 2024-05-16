using AutoMapper;
using TaskCreationService.Data;
using TaskCreationService.Models;
using TaskCreationService.Models.DTO;
using TaskCreationService.Services.Contract;

namespace TaskCreationService.Services.Implementation
{
    public class CreateTaskServices : ITaskCreation
    {
        private readonly AppDbContext _appDbContext;
        private readonly IGetUserDetailsServices _userDetails;
        private readonly IMapper _mapper;

        public CreateTaskServices(AppDbContext appDbContext, IMapper mapper, IGetUserDetailsServices userDetails)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
            _userDetails = userDetails;
        }

        public async Task<string> CreateTask(UserTaskDto task)
        {
            var user = await _userDetails.GetById(task.UserId);
            if (user.UserId != null)
            {
                var tasks = _mapper.Map<UserTask>(task);
                await _appDbContext.UserTasks.AddAsync(tasks);
                await _appDbContext.SaveChangesAsync();
                return "Task created successfully.";
            }
            else
            {
                return "User not exist.";
            }
        }
    }
}
