using QuanLyGameConsole.Models.Momo;
using QuanLyGameConsole.Models.ViewModels;

namespace QuanLyGameConsole.Services
{
    public interface IMomoService
    {
        Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(CheckoutVM model);
        MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection);
    }
}
