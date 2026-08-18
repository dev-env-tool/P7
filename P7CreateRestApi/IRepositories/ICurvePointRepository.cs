//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;


namespace P7CreateRestApi.IRepositories
{
    public interface ICurvePointRepository
    {
        Task<IEnumerable<CurvePoint>> GetAllCurvePoints();
        Task<IEnumerable<CurvePoint>> GetCurvePointById(int id);
        Task CreateCurvePoint(CurvePoint curvePoint);
        Task UpdateCurvePoint(CurvePoint curvePoint);
        Task DeleteCurvePointById(int id);

    }
}