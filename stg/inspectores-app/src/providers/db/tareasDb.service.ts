import { TareasRealizadasDto } from './../../models/tareas.model';
import { ToolsProvider } from './../../shared/page/tools';
import { Injectable } from '@angular/core';
import { SQLite, SQLiteObject } from '@ionic-native/sqlite';
import { Platform } from 'ionic-angular';



@Injectable()
export class TareasDbService {

  private database: SQLiteObject;
  sqlstorage: SQLite;

  constructor(
    private platform: Platform,
    private sqlite:SQLite,
    public tools: ToolsProvider
    ) {

    if (this.tools.isBrowser()) return;
    this.platform.ready().then(() => {
      this.sqlite.
      create({
        name: 'inspectores.db',
        location: 'default'
      })
      .then((db:SQLiteObject)=>{
        this.database = db;
        this.createTableInf();
        // this.database.executeSql("DROP TABLE tareaRealizadas", []);
        // this.createTables().then(()=>{
        //   //communicate we are ready!
        //   this.dbReady.next(true);
        // });
      })
    });
  }

  createTableInf() {
    try {
      return this.database.executeSql(
        `CREATE TABLE IF NOT EXISTS tareaRealizadas (
          id INTEGER PRIMARY KEY AUTOINCREMENT,
          jsonData TEXT
          );`, [])    
    }
    catch (err) {
      return console.log("Error al crear tabbla: tareaRealizadas", err);
    }
  }

  addTareas(tarea: TareasRealizadasDto) {
    let sql = "INSERT INTO tareaRealizadas (jsonData) VALUES (?);";
    this.database.executeSql(sql, [JSON.stringify(tarea)])
    .then((data) =>{
      console.log("addTareas:", data);
    }).catch(err=> 
      {
        console.log("Error insert tareaRealizadas");
        console.log(err);
      }
      );
      
  }

  getTareas(){
    let sql = "SELECT * FROM tareaRealizadas";
    return this.database.executeSql(sql, [])
    .then(data => {
      console.log("HAY DATOS EN TAREAS CACHE:", data.rows);
      
      if (data.rows.length) {
        let tarea = JSON.parse(data.rows.item(0).jsonData);
        tarea.id = data.rows.item(0).id;

        return Promise.resolve(tarea);
      }
      else{
        return Promise.resolve(null);
      }  
    }).catch(error => Promise.reject(error));
  }

  deleteTareas(Id:any) {
    this.database.executeSql('DELETE FROM tareaRealizadas  WHERE id = ?;',[Id]).catch((error) => {
      console.log("Error al borrar tareas de cache:",error);     
    });
  }
  
}