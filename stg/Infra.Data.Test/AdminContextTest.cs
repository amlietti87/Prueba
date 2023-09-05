using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ROSBUS.infra.Data.Contexto;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using Moq;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Internal;

using Microsoft.EntityFrameworkCore.Internal;
using Remotion.Linq.Parsing.Structure;
using Microsoft.EntityFrameworkCore.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.EntityFrameworkCore.Storage;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.infra.Data.Repositories;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using System.Data.SqlTypes;
using ROSBUS.Infra.Data.Repositories;
using System.Collections.Generic;
using System.Linq.Expressions;
using TECSO.FWK.AppService;

namespace Infra.Data.Test
{
    [TestClass]
    public class AdminContextTest
    {
        [TestInitialize]
        public void Initialize()
        {

            var serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
                .Setup(x => x.GetService(typeof(IAuthService)))
                .Returns(new AuthServiceTest());

            ServiceProviderResolver.ServiceProvider = serviceProvider.Object;

            var cnnStr = @"Server=172.25.1.28;Database=Rosbus_Int;User Id=tecso; Password=t3cs0c00p ;MultipleActiveResultSets=true";

            var optionsBuilder = new DbContextOptionsBuilder<AdminContext>();
            optionsBuilder.UseSqlServer(cnnStr);
            _context = new AdminContext(optionsBuilder.Options);

            var optionsBuilderAdjunto = new DbContextOptionsBuilder<AdjuntosContext>();
            optionsBuilderAdjunto.UseSqlServer(cnnStr);
            _adjuntoscontext = new AdjuntosContext(optionsBuilderAdjunto.Options);

            var OperacionesRBContext = new DbContextOptionsBuilder<OperacionesRBContext>();
            OperacionesRBContext.UseSqlServer("Server =172.16.17.59;Database=operacionesRB;User Id=sa; Password=Pa$$w0rd");
            _OperacionesRBContext = new OperacionesRBContext(OperacionesRBContext.Options);

            serviceProvider
                .Setup(x => x.GetService(typeof(IAdminDBResilientTransaction)))
                .Returns(new AdminDBResilientTransaction(_context));


            serviceProvider
               .Setup(x => x.GetService(typeof(ILogger)))
               .Returns(new LogServiceDebug());

            serviceProvider
                .Setup(x => x.GetService(typeof(IRutasRepository)))
                .Returns(new RutasRepository(_context, new AdminDBResilientTransaction(_context)));

            serviceProvider
                .Setup(x => x.GetService(typeof(IPuntosRepository)))
                .Returns(new PuntosRepository(_context));

            serviceProvider
               .Setup(x => x.GetService(typeof(IBanderaRepository)))
               .Returns(new BanderaRepository(_context));

            //serviceProvider
            //   .Setup(x => x.GetService(typeof(IPlaDistribucionDeCochesPorTipoDeDiaRepository)))
            //   .Returns(new PlaDistribucionDeCochesPorTipoDeDiaRepository(_context, new HServiciosRepository(_context)));

            serviceProvider
                .Setup(x => x.GetService(typeof(ISysParametersRepository)))
                .Returns(new SysParametersRepository(_context));

            serviceProvider
                .Setup(x => x.GetService(typeof(ISysDataTypesRepository)))
                .Returns(new SysDataTypesRepository(_context));

            serviceProvider
                .Setup(x => x.GetService(typeof(IRamalColorRepository)))
                .Returns(new RamalColorRepository(_context));

            serviceProvider
               .Setup(x => x.GetService(typeof(IHFechasConfiRepository)))
               .Returns(new HFechasConfiRepository(_context, null));



            ServiceProviderResolver.ServiceProvider.GetService<IAuthService>();
        }

        public AdminContext _context { get; set; }

        public AdjuntosContext _adjuntoscontext { get; set; }
        public OperacionesRBContext _OperacionesRBContext { get; set; }


        [TestMethod]
        public async Task HBasec_Include_HKilometrosNavigation()
        {
            //Arrange
            var hFechasConfiRepository = ServiceProviderResolver.ServiceProvider.GetService<IHFechasConfiRepository>();
            var r = await hFechasConfiRepository.GetByIdAsync(7227);
            var firsthbasec = r.HBasec.FirstOrDefault();

            //Act
            firsthbasec.Km = new Random(1).Next(1, 99999);
            await hFechasConfiRepository.UpdateAsync(r);

            //Assert
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void CroTipo_Include_CroElemeneto()
        {
            var r1 = _context.CroTipo.Include(e => e.CroElemeneto).ToList();
            Assert.IsTrue(r1.Any(a => a.CroElemeneto.Any()));
        }

        [TestMethod]
        public void Obtener_Sectores_Tarifarios_Desde_AdminContext()
        {
            var r1 = _context.BolSectoresTarifarios.ToList();
            Assert.IsNotNull(r1);
        }

        [TestMethod]
        public void Obtener_sys_DataTypes_Desde_AdminContext()
        {
            //Arrange
            var r1 = _context.SysDataTypes.ToList();
            //Act
            //Assert
            Assert.IsNotNull(r1);
        }

        [TestMethod]
        public async Task Obtener_sys_datatype_desde_Repository()
        {
            //Arrange
            var sysDataTypes = ServiceProviderResolver.ServiceProvider.GetService<ISysDataTypesRepository>();
            //Act
            var r = await sysDataTypes.GetAllAsync(e => true);
            //Assert
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void Obtener_sys_parameters_Desde_AdminContext()
        {
            //Arrange
            var r1 = _context.SysParameters.ToList();
            //Act
            //Assert
            Assert.IsNotNull(r1);
        }


        [TestMethod]
        public async Task Obtener_sys_parametros_desde_Repository()
        {
            //Arrange
            var sysParameters = ServiceProviderResolver.ServiceProvider.GetService<ISysParametersRepository>();
            //Act
            var r = await sysParameters.GetAllAsync(e => true);
            //Assert
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public async Task Obtener_sys_parametros_desde_Repository_con_include()
        {
            //Arrange
            var sysParameters = ServiceProviderResolver.ServiceProvider.GetService<ISysParametersRepository>();
            var include = new List<Expression<Func<SysParameters, object>>>
            {
                e=> e.SysDataType
            };
            //Act
            var r = await sysParameters.GetAllAsync(e => true, include);
            //Assert
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public async Task Obtener_ramalcolor_desde_Repository_con_include()
        {
            //Arrange
            var ramalColorRepository = ServiceProviderResolver.ServiceProvider.GetService<IRamalColorRepository>();
            var include = new List<Expression<Func<PlaRamalColor, object>>>
            {
                e=> e.HBanderas

            };
            //Act
            var r = await ramalColorRepository.GetAllAsync(e => true, include);
            //Assert
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void HFechasConfiRepository_GetLineasHorarias()
        {

            var repo = new HFechasConfiRepository(_context, new HServiciosRepository(_context));


            var var = repo.GetLineasHorarias().Result;


        }


        [TestMethod]
        public async Task HFechasConfiRepository_ObtenerDestinatarios()
        {

            var repo = new HFechasConfiRepository(_context, new HServiciosRepository(_context));


            var var = await repo.ObtenerDestinatarios(1);


            Assert.IsNotNull(var);


        }

        [TestMethod]
        public void _OperacionesRBContext_Empleados()
        {

            var a = _OperacionesRBContext.Empleados.Count();

            var bb = _OperacionesRBContext.Empleados.Where(e => e.Nombre.Contains("a"));

            var l = bb.ToList();

            var c = 1;
        }


        [TestMethod]
        public void ObtenerElPrimerRegistodehorarios()
        {

            var rx = _context.HFechasConfi.FirstOrDefault();
            var i5 = _context.PlaEstadoHorarioFecha.FirstOrDefault();
            var r1 = _context.HSectores.FirstOrDefault();
            var r2 = _context.HMediasVueltas.FirstOrDefault();
            var r3 = _context.HMinxtipo.FirstOrDefault();
            var r4 = _context.HProcMin.FirstOrDefault();
            var r5 = _context.HDetaminxtipo.FirstOrDefault();
            var r6 = _context.GpsDetaReco.FirstOrDefault();
        }


        [TestMethod]
        public void Add_addjuntofiler()
        {

            var a = new Adjuntos();
            a.Nombre = "elian";
            a.Archivo = File.ReadAllBytes(@"C:\1.pdf");


            var r = this._adjuntoscontext.Adjuntos.Add(a);
            this._adjuntoscontext.SaveChanges();

            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void ObtenerLaprimeraLinea()
        {
            var r = _context.Linea.FirstOrDefault();
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void ObtenerElPrimerPuntos()
        {
            var r = _context.PlaPuntos.FirstOrDefault();
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void ObtenerLineaconSucursal()
        {
            var nrolin = _context.SucursalesxLineas.FirstOrDefault();
            var r = _context.Linea.Where(e => e.Id == nrolin.CodLinea).Include(e => e.SucursalesxLineas).FirstOrDefault();
            Assert.IsTrue(r.SucursalesxLineas.Any());
        }


        [TestMethod]
        public void ObtenerLineasDeSantaFe()
        {
            var r = _context.Linea.Where(e => e.SucursalesxLineas.Any(s => s.Id == 1)).ToList();
            Assert.IsTrue(r.Any());
        }


        [TestMethod]
        public void ObtenerBanderasDeSantaFe()
        {
            var r = _context.HBanderas.Where(e => e.RamalColor.PlaLinea.SucursalId == 3).ToList();
            Assert.IsTrue(r.Any());
        }

        [TestMethod]
        public void GetEmpresas()
        {
            var r = _context.Empresa.FirstOrDefault();
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void GetPlaTipoLinea()
        {
            var r = _context.PlaTipoLinea.FirstOrDefault();
            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void GetPlaRamalColor()
        {
            var query = _context.Set<PlaRamalColor>().AsQueryable();

            //var str = query.ToSql();
            var str2 = query.ToString();

            var r = query.FirstOrDefault();

            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void GetGpsRecorridos()
        {
            var r = _context.GpsRecorridos.Include(e => e.Puntos).Where(e => e.Id == 1);

            //var sql = ((System.Data.Objects.ObjectQuery)query).ToTraceString();
            var sql = r.ToSql();

            var result = r.ToList();

            var pt = result.FirstOrDefault().Puntos.Count();

            Assert.IsNotNull(r);
        }


        [TestMethod]
        public void GetGpsDetaReco()
        {
            var r = _context.GpsDetaReco.FirstOrDefault();

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void GetHBanderas()
        {
            var r = _context.HBanderas.Include(e => e.Rutas).FirstOrDefault();

            Assert.IsNotNull(r);
        }


        [TestMethod]
        public async Task InsertarHBanderas()
        {

            BanderaRepository banderaRepository = new BanderaRepository(_context);

            HBanderas banderas = new HBanderas();

            banderas.AbrBan = "Abr";
            banderas.Activo = true;
            banderas.ClaveWay = "Cla";

            banderas.CodigoVarianteLinea = "Codi";
            banderas.DesBan = "DesBan";
            banderas.Ramalero = "Ram";
            banderas.SenBan = "Sen";
            banderas.TipoBanderaId = 1;


            var r = await banderaRepository.AddAsync(banderas);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public async Task InsertarRecorrido()
        {

            var recorridoRepo = ServiceProviderResolver.ServiceProvider.GetService<IRutasRepository>();

            //HBanderas banderas = await banderaRepository.GetByIdAsync(1003);

            GpsRecorridos recorridos = new GpsRecorridos();


            recorridos.Activo = "1";
            recorridos.CodBan = 1003;
            recorridos.CodLin = 120;

            recorridos.CodSec = 2198;
            recorridos.EstadoRutaId = 1;
            recorridos.Fecha = DateTime.Now;

            var r = await recorridoRepo.AddAsync(recorridos);

            Assert.IsNotNull(r);
        }



        [TestMethod]
        public async Task ExistenMediasVueltasIncompletas()
        {

            var recorridoRepo = ServiceProviderResolver.ServiceProvider.GetService<IPlaDistribucionDeCochesPorTipoDeDiaRepository>();

            var result = await recorridoRepo.ExistenMediasVueltasIncompletas(new PlaDistribucionDeCochesPorTipoDeDia() { CodHfecha = 7040, CodTdia = 1 });
            var result2 = await recorridoRepo.ExistenMediasVueltasIncompletas(new PlaDistribucionDeCochesPorTipoDeDia() { CodHfecha = 7030, CodTdia = 1 });



            //HBanderas banderas = await banderaRepository.GetByIdAsync(1003);

        }





        [TestMethod]
        public async Task InsertarPunto()
        {

            var puntosRepository = ServiceProviderResolver.ServiceProvider.GetService<IPuntosRepository>();

            //HBanderas banderas = await banderaRepository.GetByIdAsync(1003);

            PlaPuntos punto = new PlaPuntos();


            punto.CodRec = 1901;
            //punto.Cuenta = 1;
            //punto.Sent1 = "E";
            //punto.Sent2 = "SE";
            //punto.Sector = "1";
            punto.Id = Guid.NewGuid();
            var r = await puntosRepository.AddAsync(punto);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public async Task UpdatePunto()
        {

            var puntosRepository = ServiceProviderResolver.ServiceProvider.GetService<IPuntosRepository>();

            //HBanderas banderas = await banderaRepository.GetByIdAsync(1003);

            PlaPuntos punto = await puntosRepository.GetByIdAsync(Guid.Empty);

            //punto.Cuenta = 2;

            var r = await puntosRepository.UpdateAsync(punto);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public async Task LeerRutaConPuntos()
        {

            var recorridoRepo = ServiceProviderResolver.ServiceProvider.GetService<IRutasRepository>();

            //HBanderas banderas = await banderaRepository.GetByIdAsync(1003);

            var f = new RutasFilter() { Id = 1901 };

            var r = await recorridoRepo.GetByIdAsync(f);


            var aa = r.Puntos.Count;


            Assert.AreEqual(aa, 2);
        }



        [TestMethod]
        public async Task LeerHbanderas()
        {

            var espe = await _context.HBanderasEspeciales.FirstOrDefaultAsync();

            var r = await _context.HBanderas.Include(e => e.BanderasEspeciales).Where(e => e.Id == espe.CodBan).FirstOrDefaultAsync();


            Assert.IsNotNull(r.BanderasEspeciales);

            Assert.IsNotNull(r);
        }

        [TestMethod]
        public async Task StringDB()
        {


            var result = await _context.Set<BolBanderasCartelDetalle>().FromSql("exec sp_H_bandera_RecuperarCartelVigente @idBandera ", new SqlParameter("idBandera", 545)).FirstOrDefaultAsync();

            var a = result.TextoBandera;

            Assert.IsNotNull(a);
        }





        [TestMethod]
        public async Task leerh_basec()
        {
            var espe = await _context.HBasec.FirstOrDefaultAsync();

            Assert.IsNotNull(espe);


        }


        [TestMethod]
        public async Task RecuperarHbasecPorLinea()
        {
            var recorridoRepo = ServiceProviderResolver.ServiceProvider.GetService<IRutasRepository>();

            var result = await recorridoRepo.RecuperarHbasecPorLinea(1);


            Assert.IsTrue(result.Any());

            Assert.AreNotEqual(result.FirstOrDefault().CodBan, 0);
        }





        [TestMethod]
        public async Task GpsDetaReco()
        {
            var recorridoRepo = this._context.GpsDetaReco.Where(e => e.CodRec == 246);

            var result = await recorridoRepo.ToListAsync();

        }




        [TestMethod]
        public async Task RecuperarHorariosSectorPorSector_Test()
        {
            var recorridoRepo = ServiceProviderResolver.ServiceProvider.GetService<IBanderaRepository>();

            var result = await recorridoRepo.RecuperarHorariosSectorPorSector(null);

            Assert.IsTrue(result.Colulmnas.Any());

        }


        [TestMethod]
        public async Task Test_FindAllAsync()
        {
            var b = new BanderaRepository(this._context);

            var result = await b.FindAllAsync(e => e.Activo == true, s => new ResultadoTest() { Algo = s.DesBan });

            Assert.IsTrue(result.Items.Any());

        }

        [TestMethod]
        public async Task Test_SinSiniestrosConsecuencias()
        {

            var siniestrosConsecuencias = await this._context.SinSiniestrosConsecuencias.Where(e => e.ConsecuenciaId == 7).ToListAsync();
            if (siniestrosConsecuencias.Count > 0)
            {
            }

            Assert.IsNotNull(siniestrosConsecuencias);

        }


        [TestMethod]
        public async Task Test_CompletarDataReco()
        {
            var id = 3279;

            var l = this._context.GpsDetaReco.Where(e => e.CodRec == id).ToList();
            this._context.RemoveRange(l);
            this._context.SaveChanges();


            var recorridoRepo = new RutasRepository(this._context, new AdminDBResilientTransaction(_context));

            var reco = this._context.GpsRecorridos.Include(e => e.Puntos).ThenInclude(e => e.PlaCoordenada).Single(s => s.Id == id);
            reco.Puntos = reco.Puntos.OrderBy(e => e.Orden).ToList();
            var r = recorridoRepo.CompletarDataReco(reco);



            await this._context.GpsDetaReco.AddRangeAsync(r);

            await this._context.SaveChangesAsync();

        }



        [TestMethod]
        public async Task Test_PlaCodigoSubeBandera()
        {
            try
            {
                var l = await this._context.PlaSentidosBanderaSube.ToListAsync();

                var x = l;
            }
            catch (Exception ex)
            {


            }



        }







        [TestMethod]
        public async Task Test_RecuperarHorariosSectorPorSectorPlano()
        {

            BanderaRepository banderaRepository = new BanderaRepository(_context);

            var filtro = new BanderaFilter();
            filtro.Fecha = new DateTime(2017, 1, 1);
            filtro.BanderasSeleccionadas = new System.Collections.Generic.List<int>() { 430, 431, 672, 674 };
            filtro.LineaId = 121;
            filtro.Plano = true;
            var result = await banderaRepository.RecuperarHorariosSectorPorSector(filtro);


            ExcelPackage paq = new ExcelPackage();
            var sheets = paq.Workbook.Worksheets.Add("Servicios");

            int row = 1;

            foreach (var registro in result.Items)
            {
                var cell = 1;


                sheets.Cells[row, cell].Value = "Servicio";
                sheets.Cells[row + 1, cell++].Value = registro.Servicio;


                sheets.Cells[row, cell].Value = "Sale";
                sheets.Cells[row + 1, cell++].Value = registro.Sale;


                foreach (var colum in registro.ColumnasDinamicas)
                {

                    sheets.Cells[row, cell].Value = string.Format("{0} {1}", colum.Key, colum.Orden);
                    sheets.Cells[row + 1, cell++].Value = colum.value;
                }

                sheets.Cells[row, cell].Value = "Llega";
                sheets.Cells[row + 1, cell++].Value = registro.Llega;

                row = row + 3;
            }


            using (var ms = new MemoryStream())
            {
                paq.SaveAs(ms);

                File.WriteAllBytes(@"c:\Plano.xlsx", ms.ToArray());
            }



        }



        [TestMethod]
        public async Task Test_RecuperarHorariosSectorPorSectorOrdenado()
        {
            BanderaRepository banderaRepository = new BanderaRepository(_context);
            var filtro = new BanderaFilter();

            filtro.Fecha = new DateTime(2018, 10, 10);
            filtro.BanderasSeleccionadas = new System.Collections.Generic.List<int>() { 545, 18 };
            filtro.LineaId = 1;

            var result = await banderaRepository.RecuperarHorariosSectorPorSector(filtro);


            ExcelPackage paq = new ExcelPackage();
            var sheets = paq.Workbook.Worksheets.Add("Servicios");

            int row = 1;


            var cell = 1;

            sheets.Cells[row + 1, cell++].Value = "Servicio";
            sheets.Cells[row + 1, cell++].Value = "Sale";

            foreach (var Co in result.Colulmnas)
            {
                sheets.Cells[row + 1, cell++].Value = Co.Label;
            }

            sheets.Cells[row + 1, cell++].Value = "Llega";
            sheets.Cells[row + 1, cell++].Value = "Bandera";
            sheets.Cells[row + 1, cell++].Value = "Horden Hora";
            row = row + 1;


            foreach (var registro in result.Items)
            {
                cell = 1;

                sheets.Cells[row + 1, cell++].Value = registro.Servicio;
                sheets.Cells[row + 1, cell++].Value = registro.Sale;

                foreach (var colum in registro.ColumnasDinamicas)
                {
                    sheets.Cells[row + 1, cell++].Value = string.Format("M:{0} , H: {1}", colum.value, colum.Hora);
                }

                sheets.Cells[row + 1, cell++].Value = registro.Llega;
                sheets.Cells[row + 1, cell++].Value = registro.Bandera;
                sheets.Cells[row + 1, cell++].Value = registro.HoraPatron;

                row = row + 1;
            }


            using (var ms = new MemoryStream())
            {
                paq.SaveAs(ms);

                File.WriteAllBytes(@"c:\1\Ordenado.xlsx", ms.ToArray());
            }



        }




        public class ResultadoTest
        {
            public string Algo { get; set; }
        }


    }




    public class AuthServiceTest : IAuthService
    {
        public string GetCurretToken()
        {
            return "Test";
        }

        public int GetCurretUserId()
        {
            return 1;
        }

        public string GetCurretUserName()
        {
            return "test";
        }

        public string GetSessionID()
        {
            return "Test";
        }
    }



    public static class IQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly PropertyInfo DatabaseDependenciesField
            = typeof(Microsoft.EntityFrameworkCore.Storage.Database).GetTypeInfo().DeclaredProperties.Single(x => x.Name == "Dependencies");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
            {
                throw new ArgumentException("Invalid query");
            }

            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);
            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = ((DatabaseDependencies)DatabaseDependenciesField.GetValue(database)).QueryCompilationContextFactory;
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);
            var sql = modelVisitor.Queries.First().ToString();

            return sql;
        }
    }

}
