using System.Text.Json;
using StackExchange.Redis;

namespace Dometrain.Monolith.Api.Courses;

public class CachedCourseRepository : ICourseRepository
{
    private readonly ICourseRepository _courseRepository;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CachedCourseRepository(ICourseRepository courseRepository, IConnectionMultiplexer connectionMultiplexer)
    {
        _courseRepository = courseRepository;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<Course?> CreateAsync(Course course)
    {
        var created = await _courseRepository.CreateAsync(course);
        if (created is null)
        {
            return created;
        }

        var db = _connectionMultiplexer.GetDatabase();
        var serializerCourse = JsonSerializer.Serialize(course);
        await db.StringSetAsync($"course_id_{course.Id}", serializerCourse);
        return created;
    }

    public Task<Course?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Course?> GetBySlugAsync(string slug)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Course>> GetAllAsync(string nameFilter, int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<Course?> UpdateAsync(Course course)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
