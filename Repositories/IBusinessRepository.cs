using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Backend.Data;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Repositories;

public interface IBusinessRepository
{
    Task CreateBusiness(CreateBusinessDTO createBusinessDTO);
    Task<Business?> GetBusinessByIdAsync(string userId);
}
public class BusinessRepository : IBusinessRepository
{

    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public BusinessRepository(AppDbContext context , IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }
    public async Task CreateBusiness(CreateBusinessDTO createBusinessDTO)
    {
        try
        {
            var newBusiness = _mapper.Map<Business>(createBusinessDTO);

            await _context.Businesses.AddAsync(newBusiness);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<Business?> GetBusinessByIdAsync(string userId)
    {
        try
        {
            return await _context.Businesses.FirstOrDefaultAsync(b => b.UserId == userId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
