using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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