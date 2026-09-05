using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HLChitFund.Application.DTOs.Winner;

namespace HLChitFund.Application.Common.Interfaces;

public interface IWinnerService
{
    Task<IEnumerable<WinnerResponseDto>> GetAllWinnersAsync();
    Task<IEnumerable<WinnerResponseDto>>
        GetWinnersByChitGroupAsync(int chitGroupId);
    Task<WinnerResponseDto?> GetWinnerByIdAsync(int id);
    Task<WinnerResponseDto> SelectWinnerAsync(
        WinnerRequestDto request);
    Task<bool> DeleteWinnerAsync(int id);
}