using HLChitFund.Application.DTOs.ChitGroup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HLChitFund.Application.Common.Interfaces.Repositories
{
    public interface IChitGroupService
    {
        Task<IEnumerable<ChitGroupResponseDto>> GetAllChitGroupsAsync();
        Task<ChitGroupResponseDto?> GetChitGroupByIdAsync(int id);
        Task<ChitGroupResponseDto> CreateChitGroupAsync(
            ChitGroupRequestDto request);
        Task<ChitGroupResponseDto> UpdateChitGroupAsync(
            int id, ChitGroupRequestDto request);
        Task<bool> DeleteChitGroupAsync(int id);
        Task<bool> ToggleChitGroupStatusAsync(int id);
    }
}
