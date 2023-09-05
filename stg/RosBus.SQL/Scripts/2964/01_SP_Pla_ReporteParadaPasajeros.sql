ALTER PROCEDURE [dbo].[sp_pla_ReporteParadasPasajeros]
(
	 @LineaId DECIMAL,
	 @Banderas NVARCHAR(MAX)	
)
AS 
BEGIN
	DECLARE @tablaIds TABLE (Id INT)
	
	INSERT INTO @tablaIds (Id)
	SELECT CAST(fs.val AS INT)  FROM dbo.fn_Split(@Banderas,',') AS fs
	
	SELECT 
	l.cod_lin AS LineaId,
	l.des_lin AS Linea,
	prc.Id AS RamalId,
	prc.Nombre AS Ramal,
	hb.cod_ban AS BanderaId,
	hb.abr_ban AS Bandera,
	hb.DescripcionPasajeros AS DescripcionPasajeros,
	pp2.Codigo AS CodigoParada,
	UPPER(TRIM(l2.dsc_localidad)) AS Localidad,
	UPPER(TRIM(pp2.Calle)) AS Calle,
	UPPER(TRIM(pp2.Cruce)) AS Cruce,
	gr.Nombre as NombreMapa	
	FROM linea AS l
	JOIN pla_RamalColor AS prc ON prc.LineaId = l.cod_lin
	JOIN h_banderas AS hb ON hb.RamalColorID = prc.Id
	JOIN gps_recorridos AS gr ON gr.cod_ban = hb.cod_ban
	JOIN pla_Puntos AS pp ON pp.CodRec = gr.cod_rec
	JOIN pla_Paradas AS pp2 ON pp2.Id = pp.Pla_ParadaId
	JOIN OperacionesRB.dbo.localidades AS l2 ON l2.cod_localidad = pp2.LocalidadId
	WHERE 
	L.cod_lin = @LineaId
	AND gr.IsDeleted = 0
	AND gr.EstadoRutaId = 2
	AND cast(GETDATE() AS DATE) >= CAST(gr.fecha AS DATE)
	AND (cast(gr.FechaVigenciaHasta AS DATE) IS NULL OR  cast(gr.FechaVigenciaHasta AS DATE) >= cast(GETDATE() AS DATE))
	AND hb.cod_ban in (SELECT Id FROM @tablaIds)
	ORDER BY pp.Orden
  
END