var chalk = require("chalk");
var fs = require('fs');
var path = require('path');
var useDefaultConfig = require('@ionic/app-scripts/config/webpack.config.js');

//no sabemos porque cuando se tira el comando para GENERAR EL APK (ionic cordova build android  --prod)
//se esta trayendo cargada solo la variable de entorno IONIC_ENV
var envIonic = process.env.IONIC_ENV;
//Cuando corremos la app para poder debuguearla con este comando (npm  run ionic:prod)
//nos esta tomando el parametro prod en esta variable de entorno process.env.MY_ENV
var env = process.env.MY_ENV;

var aliasEnv=env
if(env==undefined)
{
  if(envIonic!=undefined){
    env=envIonic;
    aliasEnv=env;
  }
  else{
    //si no podemos obtener el environment desde las variables de entorno le seteamos por defaul dev
    env='dev';
    aliasEnv='dev';
  }  
  
}

console.log(chalk.green('\n' +  path.resolve(environmentPath(env)) + '  path.resolve(environmentPath(env)v!'));

  useDefaultConfig[aliasEnv] = useDefaultConfig.dev;
  useDefaultConfig[aliasEnv].resolve.alias = {
     "@app/env": path.resolve(environmentPath(env))
   };
   

function environmentPath(env) {

  var filePath = './src/environments/environment' + (env === '' || env === undefined ? '' : '.' + env) + '.ts';
  if (!fs.existsSync(filePath)) { 
    debugger;
    console.log(chalk.red('\n' + filePath + ' does not exist!'));
  } else {
    return filePath;
  }
}

module.exports = function () {
  return useDefaultConfig;
};

// const path = require('path');

// //const webpackConfig = require('../node_modules/@ionic/app-scripts/config/webpack.config');
// var useDefaultConfig = require('@ionic/app-scripts/config/webpack.config.js');

// useDefaultConfig['prod'].resolve.alias = {
//   "@app/env": path.resolve('./src/environments/environment.prod.ts')
// }
// useDefaultConfig['dev'].resolve.alias = {
//   "@app/env": path.resolve('./src/environments/environment.dev.ts')
// }

