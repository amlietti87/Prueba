/************************************************************
 * Code formatted by SoftTree SQL Assistant © v9.2.349
 * Time: 28/02/2019 10:43:29
 ************************************************************/
ALTER PROCEDURE [dbo].[sp_pla_ExistenMediasVueltasIncompletas] 
(
	@cod_hfecha INT ,
	@cod_tdia INT,
	@banderaList NVARCHAR(MAX)
)
AS
BEGIN
    /*******************************************
    *         
    Incompleta = 0,
    SinMinutos = 1,
    Valido = 2
    * 
    *******************************************/
    DECLARE @result INT=0;  
    
    
    DECLARE @banderaTable AS TABLE (cod_ban INT)
    
    
    IF ISNULL(@banderaList, '') <> ''  
    BEGIN
    	INSERT INTO @banderaTable
    	(
    		cod_ban
    	)
    	SELECT 
    		CAST(fs.val AS INT) 
    	FROM dbo.fn_Split(@banderaList,',') AS fs
    END
    
    
    SELECT TOP 1 @result = 1
    FROM   h_horarios_confi            AS hhc
           JOIN h_servicios            AS hs ON  hs.cod_hconfi = hhc.cod_hconfi
           LEFT JOIN h_medias_vueltas  AS hmv          ON hmv.cod_servicio = hs.cod_servicio
    WHERE  hhc.cod_hfecha = @cod_hfecha
           AND hhc.cod_tdia = @cod_tdia
           AND (ISNULL(@banderaList, '') = '' OR EXISTS (SELECT 1 FROM @banderaTable AS bt WHERE bt.cod_ban = hmv.cod_ban));
    
    IF @result=0
    BEGIN
        SELECT @result;
    END
    ELSE
        SET @result = 2
    
    SELECT TOP 1 @result = 1
    FROM   h_medias_vueltas       AS hmv
           JOIN h_servicios       AS hs
                ON  hs.cod_servicio = hmv.cod_servicio
           JOIN h_horarios_confi  AS hhc
                ON  hhc.cod_hconfi = hs.cod_hconfi
             OUTER APPLY
                (
					SELECT 
							COUNT(hpm.cod_hsector) cantSec
					FROM   h_proc_min  AS hpm
					WHERE  hpm.cod_mvuelta = hmv.cod_mvuelta
						  
						 
                )  SectoresXMediaVuelta
                
    WHERE  NOT EXISTS (
               SELECT TOP 1          1
               FROM   h_proc_min  AS hpm
               WHERE  hpm.cod_mvuelta = hmv.cod_mvuelta
                      AND  (hpm.minuto <> 0 
                      OR SectoresXMediaVuelta.cantSec=2
                      ) 
           )
           AND hhc.cod_hfecha = @cod_hfecha
           AND hhc.cod_tdia = @cod_tdia
           AND (ISNULL(@banderaList, '') = '' OR EXISTS (SELECT 1 FROM @banderaTable AS bt WHERE bt.cod_ban = hmv.cod_ban));
    
    
    
    SELECT @result;
 
 
 END
 
