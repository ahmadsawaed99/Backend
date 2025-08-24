using System;
using MathNet.Numerics.Random;
using Backend.Models.HelpersModels;

namespace Backend.Services;

public interface IUtilitiesService
{
    Task SaveOtpCode(string email);
}
public class UtilitiesService
{
    private readonly IMongoDBService<Otp> _mongoDBService;

    public UtilitiesService(IMongoDBService<Otp> mongoDBService)
    {
        _mongoDBService = mongoDBService;
    }

    public async Task SaveOtpCode(string email){
        var code = CreateOtpCode();
        var otp = new Otp(){
            Email = email,
            Code = code,
            SendDate = DateTime.Now.ToUniversalTime()
        };

        await _mongoDBService.AddToDatabase(otp , "otpEmail");
    }

    private int CreateOtpCode()
    {
        var random = new Random();
        var code = random.Next(100000,99999);

        return code;
    }
}
