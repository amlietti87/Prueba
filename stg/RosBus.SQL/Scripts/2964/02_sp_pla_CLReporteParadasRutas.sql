ALTER PROCEDURE [dbo].[sp_pla_CLReporteParadasRutas]
(
	 @CodBan INT
	 ,@RoutesName NVARCHAR(MAX)-- = '125'
)
AS 
BEGIN
	
DECLARE @CodLinCodBanTable TABLE (
	codban INT 		
)

IF @RoutesName <> ''
	BEGIN
		INSERT INTO @CodLinCodBanTable (codban)
		select hb.cod_ban from h_banderas hb
		inner join pla_ramalColor prc on prc.id = hb.ramalColorId
		where
		hb.IsDeleted=0 and
		prc.IsDeleted=0 and
		prc.route_long_name in (SELECT LTRIM(RTRIM(fs.val)) from  dbo.fn_Split(@RoutesName,',') AS fs)
	END
ELSE
	BEGIN
	    INSERT INTO @CodLinCodBanTable (codban)
	    VALUES (@CodBan)
	END


SELECT
l.cod_lin AS LineaId,
l.des_lin AS Linea,
prc.Id AS RamalId,
prc.Nombre AS Ramal,
hb.cod_ban AS BanderaId,
hb.abr_ban AS Bandera,
hb.DescripcionPasajeros AS DescripcionPasajeros,
pp.Codigo AS CodigoParada, 
UPPER(TRIM(vrrd.localidadParada)) AS Localidad,
UPPER(TRIM(vrrd.calleParada)) AS Calle,
UPPER(TRIM(vrrd.cruceParada)) AS Cruce,
gr.Nombre as NombreMapa	
FROM linea AS l
JOIN pla_Linea_LineaHoraria pllh ON pllh.LineaId = l.cod_lin
JOIN pla_Linea pl ON pl.id = pllh.pla_LineaId
JOIN pla_RamalColor AS prc ON  prc.LineaId = pl.Id
JOIN h_banderas AS hb ON hb.RamalColorID = prc.id

JOIN ver_recorridosRespaldo AS r ON r.cod_lin = prc.LineaId and r.cod_ban = hb.cod_ban
JOIN gps_recorridos AS gr ON gr.cod_rec=r.cod_rec
JOIN ver_recorridosRespaldo_det AS vrrd ON vrrd.cod_ban = r.cod_ban AND vrrd.cod_hfecha = r.cod_hfecha AND vrrd.cod_version = r.cod_version
JOIN pla_Paradas AS pp ON pp.id = vrrd.paradaId
JOIN ver_versiones AS v ON v.cod_linea = r.cod_lin and r.cod_version = v.cod_version
WHERE activada = 1
AND cast(GETDATE() AS DATE) >= fec_activacion
AND (cast(GETDATE() AS DATE) <= fec_caducidad or
fec_caducidad is null)
AND v.cod_tipoVersion = 10
AND vrrd.EsParada = 1
AND hb.cod_ban in (SELECT codban FROM @CodLinCodBanTable)
ORDER BY vrrd.cuenta
 
END	
