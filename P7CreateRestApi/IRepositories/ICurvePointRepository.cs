//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;


namespace P7CreateRestApi.IRepositories
{
    public interface ICurvePointRepository
    {
        Task<IEnumerable<CurvePoint>> GetAllCurvePoints();
        Task<IEnumerable<CurvePoint>> GetCurvePointById(int id);
        void CreateCurvePoint(CurvePoint curvePoint);
        Task UpdateCurvePoint(CurvePoint curvePoint);
        void DeleteCurvePointById(int id);

    }
}