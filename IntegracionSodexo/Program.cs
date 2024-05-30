using Dapper;
using IntegracionSodexo;
using System;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.Data;
using System.Data.Common;
using IntegracionSodexo.Model;
using System.Xml.Linq;
using IntegracionSodexo.Controller;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

SqlConnection conectar = Connection.Obtenerconexion();

String? fecha1 = "";
String? fecha2 = "";

Console.Write("Realizar Consulta Manual S/N?: ");
String? entrada = Console.ReadLine();    

while (!entrada.ToLower().Equals("n"))
{
    Console.WriteLine("Ingresar Fecha Inicial:");
    fecha1 = Console.ReadLine();
    Console.WriteLine("Ingresar Fecha Final:");
    fecha2 = Console.ReadLine();
    entrada = "n";
    Console.Clear();
}
using (var db =  conectar)
{
    if(fecha1 == ""){
        var sql = "SELECT DISTINCT OC.IDORGC AS CC, USUARIOS.EMAIL, OC.PORCIMPUESTO, OC.TOTAL, OC.IDESTADODOC, OC.COMENTARIOS, OC.IDMONEDA, CASE WHEN charindex('-', OC.RUTORGV) > 0 THEN( SUBSTRING(OC.RUTORGV, CHARINDEX('-', OC.RUTORGV) - 9, 9))ELSE OC.RUTORGV END AS RUTORGV,CASE WHEN charindex('-', OC.RUTORGV) > 0 THEN (SUBSTRING(OC.RUTORGV, CHARINDEX('-', OC.RUTORGV) + 1, LEN(OC.RUTORGV))) ELSE '0' END AS digito, SUBSTRING(OC.RUT, CHARINDEX('-', OC.RUT) - 9, \r\n                         9) AS RUT, SUBSTRING(OC.NUMOC, CHARINDEX('-', OC.NUMOC) + 1, LEN(OC.NUMOC)) AS NUMOC, SUBSTRING(OC.NUMOC, CHARINDEX('-', OC.NUMOC) - 5, 5) AS PREFIJO, ORGV.TELEFONO,ORGV.EMAIL AS Correo_provee, CONVERT(VARCHAR(50), OC.FECHAENVIO, 127) AS FECHAENVIO, OCLINEAS.CANTIDAD, OCLINEAS.IDARTICULO, OCLINEAS.MONTOLINEA, OCLINEAS.MONTOUNITARIO,OCLINEAS.NOMBARTICULO, OCLINEAS.DESCUENTOS, OCLINEAS.IVALINEA, OCLINEAS.PORCIMPUESTO AS Expr1, DIRECCION.DIRECCION1, CONVERT(VARCHAR(50), OC.FECHACREACION, 127) AS FECHACREACION, FORMASPAGO.DESCRIPCORTA, OC.IDEMPRESAV, OCLNDISTRIBUCION.IDCENTCOSTO, ORGCCENTROCOSTO.DESCRIPCION, FORMASPAGO.DESCRIPLARGA, EMPRESAS.NOMBFANTASIA,COMUNA.CODSII as CIUDAD_DANE,SUBSTRING(CAST(COMUNA.CODSII AS varchar(38)), 1, 2) AS DEPARTAMENTO_DANE FROM OC INNER JOIN OCLINEAS ON OC.IDEMPRESA = OCLINEAS.IDEMPRESA AND OC.IDORGC = OCLINEAS.IDORGC AND OC.IDOC = OCLINEAS.IDOC INNER JOIN USUARIOS ON OC.IDEMPRESA = USUARIOS.IDEMPRESA AND OC.IDUSUARIO = USUARIOS.IDUSUARIO INNER JOIN ORGV ON OC.IDORGV = ORGV.IDORGV AND OC.IDEMPRESAV = ORGV.IDEMPRESA INNER JOIN ORGVDIRECCION ON ORGV.IDEMPRESA = ORGVDIRECCION.IDEMPRESA AND ORGV.IDORGV = ORGVDIRECCION.IDORGV INNER JOIN DIRECCION ON ORGVDIRECCION.IDEMPRESA = DIRECCION.IDEMPRESA AND ORGVDIRECCION.IDDIRECCION = DIRECCION.IDDIRECCION INNER JOIN OCLNDISTRIBUCION ON OCLINEAS.IDEMPRESA = OCLNDISTRIBUCION.IDEMPRESA AND OCLINEAS.IDORGC = OCLNDISTRIBUCION.IDORGC AND OCLINEAS.IDOC = OCLNDISTRIBUCION.IDOC INNER JOIN ORGCCENTROCOSTO ON OCLNDISTRIBUCION.IDEMPRESA = ORGCCENTROCOSTO.IDEMPRESA AND OCLNDISTRIBUCION.IDORGC = ORGCCENTROCOSTO.IDORGC AND OCLNDISTRIBUCION.IDCENTCOSTO = ORGCCENTROCOSTO.IDCENTCOSTO INNER JOIN EMPRESAS ON OC.IDEMPRESA = EMPRESAS.IDEMPRESA INNER JOIN\r\n                         COMUNA ON DIRECCION.IDCOMUNA = COMUNA.IDCOMUNA LEFT OUTER JOIN FORMASPAGO ON OC.IDFORMAPAGO = FORMASPAGO.IDFORMAPAGO WHERE (OC.IDEMPRESA = '42608')  AND (OC.IDESTADODOC = 46 OR OC.IDESTADODOC = 47 OR OC.IDESTADODOC = 49) AND (CONVERT(date, OC.FECHAENVIO, 23) between convert(date,CURRENT_TIMESTAMP) and convert(date,CURRENT_TIMESTAMP))";
        var mapeo = db.Query<Mapeo>(sql);
        Console.WriteLine("Procesando Archivo xlsx");
        ExcelPackage.LicenseContext = LicenseContext.Commercial;

        // If you use EPPlus in a noncommercial context
        // according to the Polyform Noncommercial license:
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        ExcelPackage ArchivoExcel = new ExcelPackage();

        //Le añadimos los 'worksheets' que necesitemos.
        //En este caso añadiremos solo uno
        ArchivoExcel.Workbook.Worksheets.Add("Sodexo");

        //Creamos un objecto tipo ExcelWorksheet para
        //manejarlo facilmente.
        ExcelWorksheet ew1 = ArchivoExcel.Workbook.Worksheets["Sodexo"];
        //ExcelWorksheet ew1 = ArchivoExcel.Workbook.Worksheets[1];
        ew1.Cells["A1"].Value = "Address1";
        ew1.Cells["B1"].Value = "BusinessUnit";
        ew1.Cells["C1"].Value = "CampoTexto3";
        ew1.Cells["D1"].Value = "CampoTexto4";
        ew1.Cells["E1"].Value = "CFD_campo_texto_9";
        ew1.Cells["F1"].Value = "CFD_campo_texto_10";
        ew1.Cells["G1"].Value = "CFD_campo_texto_20";
        ew1.Cells["H1"].Value = "CantidadDec1";
        ew1.Cells["I1"].Value = "City";
        ew1.Cells["J1"].Value = "EmailAddress";
        ew1.Cells["K1"].Value = "State";
        ew1.Cells["L1"].Value = "DV";
        ew1.Cells["M1"].Value = "IdEstiloDocumento";
        ew1.Cells["N1"].Value = "OrderDate";
        ew1.Cells["O1"].Value = "PaymentDate";
        ew1.Cells["P1"].Value = "PaymentType";
        ew1.Cells["Q1"].Value = "IDInternoCliente";
        ew1.Cells["R1"].Value = "Notes";
        ew1.Cells["S1"].Value = "Currency";
        ew1.Cells["T1"].Value = "ID";
        ew1.Cells["U1"].Value = "FullName";
        ew1.Cells["V1"].Value = "OrderNumber";
        ew1.Cells["W1"].Value = "SubTotal";
        ew1.Cells["X1"].Value = "NoExteriorSuc";
        ew1.Cells["Y1"].Value = "PhoneNumber";
        ew1.Cells["Z1"].Value = "CalcularTotales";
        ew1.Cells["AA1"].Value = "Total";
        ew1.Cells["AB1"].Value = "Doctype";
        ew1.Cells["AC1"].Value = "TotalIVA";
        ew1.Cells["AD1"].Value = "Con_dato_decimal1";
        ew1.Cells["AE1"].Value = "Con_dato_decimal2";
        ew1.Cells["AF1"].Value = "CajasTotal";
        ew1.Cells["AG1"].Value = "CodigoServientre";
        ew1.Cells["AH1"].Value = "NombreServientre";
        ew1.Cells["AI1"].Value = "DescuentoPorItem";
        ew1.Cells["AJ1"].Value = "TaxCost_COP";
        ew1.Cells["AK1"].Value = "CON_impuesto2";
        ew1.Cells["AL1"].Value = "ConceptoPorIVA";
        ew1.Cells["AM1"].Value = "ConceptoPor1";
        ew1.Cells["AN1"].Value = "TotalConcepto";
        ew1.Cells["AO1"].Value = "DeclaredValue_COP";

        int fila = 2;
        foreach (var Olement in mapeo)
        {
            ew1.Cells["A" + fila].Value = Olement.DIRECCION1;
            ew1.Cells["B" + fila].Value = "T";
            ew1.Cells["C" + fila].Value = Olement.IDCENTCOSTO;
            ew1.Cells["D" + fila].Value = Olement.EMAIL;
            ew1.Cells["E" + fila].Value = "A";
            ew1.Cells["F" + fila].Value = "";
            ew1.Cells["G" + fila].Value = Olement.PREFIJO + Olement.NUMOC;
            ew1.Cells["H" + fila].Value = Olement.PORCIMPUESTO;
            ew1.Cells["I" + fila].Value = "11001";
            ew1.Cells["J" + fila].Value = Olement.Correo_provee;
            ew1.Cells["K" + fila].Value = Olement.DEPARTAMENTO_DANE;
            ew1.Cells["L" + fila].Value = Olement.digito;
            ew1.Cells["M" + fila].Value = "40";
            ew1.Cells["N" + fila].Value = Olement.FECHACREACION;
            ew1.Cells["O" + fila].Value = Olement.FECHAENVIO;
            ew1.Cells["P" + fila].Value = Olement.DESCRIPLARGA;
            ew1.Cells["Q" + fila].Value = Olement.RUTORGV;
            ew1.Cells["R" + fila].Value = Olement.COMENTARIOS;
            ew1.Cells["S" + fila].Value = "COP";
            ew1.Cells["T" + fila].Value = Olement.RUTORGV;
            ew1.Cells["U" + fila].Value = Olement.NOMBFANTASIA;
            ew1.Cells["V" + fila].Value = Olement.PREFIJO + "-" + Olement.NUMOC;
            ew1.Cells["W" + fila].Value = Olement.MONTOUNITARIO * Olement.CANTIDAD;
            //ew1.Cells["W" + fila].Value = Olement.TOTAL;
            ew1.Cells["X" + fila].Value = Olement.DESCRIPCION;
            ew1.Cells["Y" + fila].Value = Olement.TELEFONO;
            ew1.Cells["Z" + fila].Value = "2";
            ew1.Cells["AA" + fila].Value = Olement.TOTAL - Olement.DESCUENTOS - Olement.PORCIMPUESTO;
            //ew1.Cells["AA" + fila].Value = Olement.TOTAL - Olement.DESCUENTOS - Olement.TOTAL * Olement.PORCIMPUESTO/100;
            //ew1.Cells["AA" + fila].Value = "0";
            ew1.Cells["AB" + fila].Value = "4";
            ew1.Cells["AC" + fila].Value = "0";
            ew1.Cells["AD" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AE" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AF" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AG" + fila].Value = Olement.IDARTICULO;
            ew1.Cells["AH" + fila].Value = Olement.NOMBARTICULO;
            ew1.Cells["AI" + fila].Value = Olement.DESCUENTOS;
            ew1.Cells["AJ" + fila].Value = (Olement.IVALINEA * Olement.MONTOLINEA) + Olement.MONTOLINEA;
            ew1.Cells["AK" + fila].Value = "0";
            ew1.Cells["AL" + fila].Value = Olement.IVALINEA;
            ew1.Cells["AM" + fila].Value = "0";
            ew1.Cells["AN" + fila].Value = Olement.CANTIDAD * Olement.MONTOUNITARIO;
            ew1.Cells["AO" + fila].Value = Olement.MONTOLINEA;

            //Console.WriteLine(Olement.Nombre);
            fila++;
        }
        //ArchivoExcel.SaveAs(Directory.GetCurrentDirectory() + @"\ArchivoEbillSodexo\out.xlsx");
        ArchivoExcel.SaveAs(new FileInfo(@"C:\Archivo Excel\out.xlsx"));
    }
    else
    {
        Console.WriteLine("Consulta Realizada\nFecha Inicial: " + fecha1 + "\nFecha Final: " + fecha2 + "");
        var sql = "SELECT DISTINCT OC.IDORGC AS CC, USUARIOS.EMAIL, OC.PORCIMPUESTO, OC.TOTAL, OC.IDESTADODOC, OC.COMENTARIOS, OC.IDMONEDA, CASE WHEN charindex('-', OC.RUTORGV) > 0 THEN( SUBSTRING(OC.RUTORGV, CHARINDEX('-', OC.RUTORGV) - 9, 9))ELSE OC.RUTORGV END AS RUTORGV,CASE WHEN charindex('-', OC.RUTORGV) > 0 THEN (SUBSTRING(OC.RUTORGV, CHARINDEX('-', OC.RUTORGV) + 1, LEN(OC.RUTORGV))) ELSE '0' END AS digito, SUBSTRING(OC.RUT, CHARINDEX('-', OC.RUT) - 9,9) AS RUT, SUBSTRING(OC.NUMOC, CHARINDEX('-', OC.NUMOC) + 1, LEN(OC.NUMOC)) AS NUMOC, SUBSTRING(OC.NUMOC, CHARINDEX('-', OC.NUMOC) - 5, 5) AS PREFIJO, ORGV.TELEFONO,ORGV.EMAIL AS Correo_provee, CONVERT(VARCHAR(50), OC.FECHAENVIO, 127) AS FECHAENVIO, OCLINEAS.CANTIDAD, OCLINEAS.IDARTICULO, OCLINEAS.MONTOLINEA, OCLINEAS.MONTOUNITARIO,OCLINEAS.NOMBARTICULO, OCLINEAS.DESCUENTOS, OCLINEAS.IVALINEA, OCLINEAS.PORCIMPUESTO AS Expr1, DIRECCION.DIRECCION1, CONVERT(VARCHAR(50), OC.FECHACREACION, 127) AS FECHACREACION, FORMASPAGO.DESCRIPCORTA, OC.IDEMPRESAV, OCLNDISTRIBUCION.IDCENTCOSTO, ORGCCENTROCOSTO.DESCRIPCION, FORMASPAGO.DESCRIPLARGA, EMPRESAS.NOMBFANTASIA,COMUNA.CODSII as CIUDAD_DANE,SUBSTRING(CAST(COMUNA.CODSII AS varchar(38)), 1, 2) AS DEPARTAMENTO_DANE FROM OC INNER JOIN OCLINEAS ON OC.IDEMPRESA = OCLINEAS.IDEMPRESA AND OC.IDORGC = OCLINEAS.IDORGC AND OC.IDOC = OCLINEAS.IDOC INNER JOIN USUARIOS ON OC.IDEMPRESA = USUARIOS.IDEMPRESA AND OC.IDUSUARIO = USUARIOS.IDUSUARIO INNER JOIN ORGV ON OC.IDORGV = ORGV.IDORGV AND OC.IDEMPRESAV = ORGV.IDEMPRESA INNER JOIN ORGVDIRECCION ON ORGV.IDEMPRESA = ORGVDIRECCION.IDEMPRESA AND ORGV.IDORGV = ORGVDIRECCION.IDORGV INNER JOIN DIRECCION ON ORGVDIRECCION.IDEMPRESA = DIRECCION.IDEMPRESA AND ORGVDIRECCION.IDDIRECCION = DIRECCION.IDDIRECCION INNER JOIN OCLNDISTRIBUCION ON OCLINEAS.IDEMPRESA = OCLNDISTRIBUCION.IDEMPRESA AND OCLINEAS.IDORGC = OCLNDISTRIBUCION.IDORGC AND OCLINEAS.IDOC = OCLNDISTRIBUCION.IDOC INNER JOIN ORGCCENTROCOSTO ON OCLNDISTRIBUCION.IDEMPRESA = ORGCCENTROCOSTO.IDEMPRESA AND OCLNDISTRIBUCION.IDORGC = ORGCCENTROCOSTO.IDORGC AND OCLNDISTRIBUCION.IDCENTCOSTO = ORGCCENTROCOSTO.IDCENTCOSTO INNER JOIN EMPRESAS ON OC.IDEMPRESA = EMPRESAS.IDEMPRESA INNER JOIN COMUNA ON DIRECCION.IDCOMUNA = COMUNA.IDCOMUNA LEFT OUTER JOIN FORMASPAGO ON OC.IDFORMAPAGO = FORMASPAGO.IDFORMAPAGO WHERE (OC.IDEMPRESA = '42608')  AND (OC.IDESTADODOC = 46 OR OC.IDESTADODOC = 47 OR OC.IDESTADODOC = 49) AND convert(varchar(8), OC.FECHAENVIO, 112) between '" + fecha1 + "' and '" + fecha2 + "'";
        var mapeo = db.Query<Mapeo>(sql);
        Console.WriteLine("Procesando Archivo xlsx");
        ExcelPackage.LicenseContext = LicenseContext.Commercial;

        // If you use EPPlus in a noncommercial context
        // according to the Polyform Noncommercial license:
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        ExcelPackage ArchivoExcel = new ExcelPackage();

        //Le añadimos los 'worksheets' que necesitemos.
        //En este caso añadiremos solo uno
        ArchivoExcel.Workbook.Worksheets.Add("Sodexo");

        //Creamos un objecto tipo ExcelWorksheet para
        //manejarlo facilmente.
        ExcelWorksheet ew1 = ArchivoExcel.Workbook.Worksheets["Sodexo"];
        //ExcelWorksheet ew1 = ArchivoExcel.Workbook.Worksheets[1];
        ew1.Cells["A1"].Value = "Address1";
        ew1.Cells["B1"].Value = "BusinessUnit";
        ew1.Cells["C1"].Value = "CampoTexto3";
        ew1.Cells["D1"].Value = "CampoTexto4";
        ew1.Cells["E1"].Value = "CFD_campo_texto_9";
        ew1.Cells["F1"].Value = "CFD_campo_texto_10";
        ew1.Cells["G1"].Value = "CFD_campo_texto_20";
        ew1.Cells["H1"].Value = "CantidadDec1";
        ew1.Cells["I1"].Value = "City";
        ew1.Cells["J1"].Value = "EmailAddress";
        ew1.Cells["K1"].Value = "State";
        ew1.Cells["L1"].Value = "DV";
        ew1.Cells["M1"].Value = "IdEstiloDocumento";
        ew1.Cells["N1"].Value = "OrderDate";
        ew1.Cells["O1"].Value = "PaymentDate";
        ew1.Cells["P1"].Value = "PaymentType";
        ew1.Cells["Q1"].Value = "IDInternoCliente";
        ew1.Cells["R1"].Value = "Notes";
        ew1.Cells["S1"].Value = "Currency";
        ew1.Cells["T1"].Value = "ID";
        ew1.Cells["U1"].Value = "FullName";
        ew1.Cells["V1"].Value = "OrderNumber";
        ew1.Cells["W1"].Value = "SubTotal";
        ew1.Cells["X1"].Value = "NoExteriorSuc";
        ew1.Cells["Y1"].Value = "PhoneNumber";
        ew1.Cells["Z1"].Value = "CalcularTotales";
        ew1.Cells["AA1"].Value = "Total";
        ew1.Cells["AB1"].Value = "Doctype";
        ew1.Cells["AC1"].Value = "TotalIVA";
        ew1.Cells["AD1"].Value = "Con_dato_decimal1";
        ew1.Cells["AE1"].Value = "Con_dato_decimal2";
        ew1.Cells["AF1"].Value = "CajasTotal";
        ew1.Cells["AG1"].Value = "CodigoServientre";
        ew1.Cells["AH1"].Value = "NombreServientre";
        ew1.Cells["AI1"].Value = "DescuentoPorItem";
        ew1.Cells["AJ1"].Value = "TaxCost_COP";
        ew1.Cells["AK1"].Value = "CON_impuesto2";
        ew1.Cells["AL1"].Value = "ConceptoPorIVA";
        ew1.Cells["AM1"].Value = "ConceptoPor1";
        ew1.Cells["AN1"].Value = "TotalConcepto";
        ew1.Cells["AO1"].Value = "DeclaredValue_COP";

        int fila = 2;
        foreach (var Olement in mapeo)
        {
            ew1.Cells["A" + fila].Value = Olement.DIRECCION1;
            ew1.Cells["B" + fila].Value = "T";
            ew1.Cells["C" + fila].Value = Olement.IDCENTCOSTO;
            ew1.Cells["D" + fila].Value = Olement.EMAIL;
            ew1.Cells["E" + fila].Value = "A";
            ew1.Cells["F" + fila].Value = "";
            ew1.Cells["G" + fila].Value = Olement.PREFIJO + Olement.NUMOC;
            ew1.Cells["H" + fila].Value = Olement.PORCIMPUESTO;
            ew1.Cells["I" + fila].Value = "11001";
            ew1.Cells["J" + fila].Value = Olement.Correo_provee;
            ew1.Cells["K" + fila].Value = Olement.DEPARTAMENTO_DANE;
            ew1.Cells["L" + fila].Value = Olement.digito;
            ew1.Cells["M" + fila].Value = "40";
            ew1.Cells["N" + fila].Value = Olement.FECHACREACION;
            ew1.Cells["O" + fila].Value = Olement.FECHAENVIO;
            ew1.Cells["P" + fila].Value = Olement.DESCRIPLARGA;
            ew1.Cells["Q" + fila].Value = Olement.RUTORGV;
            ew1.Cells["R" + fila].Value = Olement.COMENTARIOS;
            ew1.Cells["S" + fila].Value = "COP";
            ew1.Cells["T" + fila].Value = Olement.RUTORGV;
            ew1.Cells["U" + fila].Value = Olement.NOMBFANTASIA;
            ew1.Cells["V" + fila].Value = Olement.PREFIJO + "-" + Olement.NUMOC;
            ew1.Cells["W" + fila].Value = Olement.TOTAL;
            ew1.Cells["X" + fila].Value = Olement.DESCRIPCION;
            ew1.Cells["Y" + fila].Value = Olement.TELEFONO;
            ew1.Cells["Z" + fila].Value = "2";
            ew1.Cells["AA" + fila].Value = "0";
            ew1.Cells["AB" + fila].Value = "4";
            ew1.Cells["AC" + fila].Value = "0";
            ew1.Cells["AD" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AE" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AF" + fila].Value = Olement.CANTIDAD;
            ew1.Cells["AG" + fila].Value = Olement.IDARTICULO;
            ew1.Cells["AH" + fila].Value = Olement.NOMBARTICULO;
            ew1.Cells["AI" + fila].Value = Olement.DESCUENTOS;
            ew1.Cells["AJ" + fila].Value = (Olement.IVALINEA * Olement.MONTOLINEA) + Olement.MONTOLINEA;
            ew1.Cells["AK" + fila].Value = "0";
            ew1.Cells["AL" + fila].Value = Olement.IVALINEA;
            ew1.Cells["AM" + fila].Value = "0";
            ew1.Cells["AN" + fila].Value = Olement.CANTIDAD * Olement.MONTOUNITARIO;
            ew1.Cells["AO" + fila].Value = Olement.MONTOLINEA;

            fila++;
        }
        //ArchivoExcel.SaveAs(new FileInfo(@"C:\Archivo Excel\out.xlsx"));
        ArchivoExcel.SaveAs(Directory.GetCurrentDirectory() + @"\ArchivoEbillSodexo\out.xlsx");
    }
}

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<MyProcessor>();
    })
    .Build();

await host.RunAsync();

public class MyProcessor : BackgroundService
{
    private readonly Microsoft.Extensions.Logging.ILogger<MyProcessor> _logger;

    public MyProcessor(ILogger<MyProcessor> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}