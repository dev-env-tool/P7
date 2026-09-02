using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IServices
{
    public interface ICurvePointService
    {
        Task<IEnumerable<CurvePointDto>> GetAllCurvePointsDto();
        Task<IEnumerable<CurvePointDto>> GetCurvePointDtoById(int id);
        Task CreateCurvePointWithCurvePointDto(CurvePointDto CurvePointDto);
        Task UpdateCurvePointWithCurvePointDto(CurvePointDto CurvePointDto);
        Task DeleteCurvePointById(int id);
        Task<CurvePointDto> MapCurvePointToCurvePointDto(CurvePoint CurvePoint);
        Task<CurvePoint> MapCurvePointDtoToCurvePoint(CurvePointDto CurvePointDto);

    }
}