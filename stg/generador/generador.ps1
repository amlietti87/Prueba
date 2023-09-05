 
 $folderworking =  (Get-Item -Path '.\' -Verbose).FullName + "\"
 Write-Host   $folderworking 


 
 
$filePath = $folderworking +"templete\" 
$fileDestination = $folderworking +"generados\" 

 
$files = Get-ChildItem -Recurse -Path $filePath | Where-Object { $_.Extension -eq ".cs" } 
 
 $name = Read-Host 'nombre de la entidad?' 
 

 $EstructureType = Read-Host 'tipo de id para la entidad por defecto int ?' 
 
    If (-Not $EstructureType) {
        $EstructureType = 'int'
}


  Write-Host "entidad: " $name


foreach ($file in $files) {  
 			
   $filename = $file.fullname
   $newFilename = $filename.Replace("Entidad", $name).Replace($filePath, $fileDestination)
  # rename-item $filename $newFilename
   #Write-Host "s: " $newFilename
   $d  =  $file.Directory.fullname.Replace($filePath, $fileDestination)
    Write-Host "s: " $d
     New-Item -ItemType Directory -Force -Path $d

   # New-Item -ItemType File -Path $destination -Force

    Copy-Item $file.fullname -Destination $newFilename -Recurse  -Force

    (Get-Content $newFilename).replace('Entidad', $name).replace("EstructureType", $EstructureType ) | Set-Content $newFilename

} 

         