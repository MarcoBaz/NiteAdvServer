import {Injectable} from "@angular/core";
import {ScriptStore,Script} from "./script.store";
import {CssStore,Css} from "./css.store";
declare var document: any;

@Injectable()
export class ScriptService {

// private scripts: Script[];
// private csss: Css[];
constructor() {

    // ScriptStore.forEach((script: any) => {
    //     this.scripts[script.name] = {
    //         loaded: false,
    //         src: script.src
    //     };
    // });
    // CssStore.forEach((css: any) => {
    //     this.csss[css.name] = {
    //         loaded: false,
    //         src: css.src
    //     };
    // });
}


loadAllScripts()
{
  var promises: any[] = [];
  var i:number =0;
  ScriptStore.forEach(script => {
    promises.push(this.loadScript(script,i));
    i +=1;
  })
  return Promise.all(promises);
}
loadAllCss()
{
  var promises: any[] = [];
  var i:number =0;
    CssStore.forEach(css => {
        promises.push(this.loadCss(css,i));
        i +=1;
    })
  return Promise.all(promises);
}
loadScript(script: Script,index:number) {
    return new Promise((resolve, reject) => {
        //resolve if already loaded
        if (script.loaded) {
            script.loaded = true;
            script.status = 'Already Loaded';
            ScriptStore[index]=script;
            resolve(script);
        }
        else {
            //load script
            let scriptEl = document.createElement('script');
            scriptEl.type = 'text/javascript';
            scriptEl.src = script.src;
            scriptEl.id = script.name;
            if (scriptEl.readyState) {  //IE
                scriptEl.onreadystatechange = () => {
                    if (scriptEl.readyState === "loaded" || scriptEl.readyState === "complete") {
                        scriptEl.onreadystatechange = null;
                        script.loaded = true;
                        script.status = 'Loaded';
                        ScriptStore[index]=script;
                        resolve(script);
                    }
                };
            } else {  //Others
                scriptEl.onload = () => {
                    script.loaded = true;
                    script.status = 'Loaded';
                    ScriptStore[index]=script;
                    resolve(script);
                };
            }
            scriptEl.onerror = (error: any) => {
                script.loaded = false;
                script.status = error;
                ScriptStore[index]=script;
                resolve(script);
            }
            document.getElementsByTagName('head')[0].appendChild(scriptEl);
        }
    });
}


loadCss(css: Css,index:number) {
    return new Promise((resolve, reject) => {
        //resolve if already loaded
        if (css.loaded) {
            css.loaded = true;
            css.status = 'Already Loaded';
            ScriptStore[index]=css;
            resolve(css);
        }
        else {
            //load script
            let link = document.createElement('link');
            link.rel = 'stylesheet';
            link.href = css.src;
            link.id = css.name;
            link.onerror = (error: any) =>{
                css.loaded = false;
                css.status = error;
                CssStore[index]=css;
                resolve(css);
            };
            document.getElementsByTagName('head')[0].appendChild(link);
            resolve(css);
        }
    });
}

unloadScripts() {
    return new Promise((resolve, reject) => {
        //resolve if already loaded
        var scripts = document.getElementsByTagName("script");
        for (var i = 0; i < scripts.length; i++) {
            var el = scripts[i];
            var baseUri =el.baseURI;
            var scriptFound = ScriptStore.find(x => x.name == el.id);
            if (scriptFound != null)
            {
                   if (el != null)
                   {
                       var index = ScriptStore.indexOf(scriptFound);
                       scriptFound.loaded = false;
                       scriptFound.status = "unloaded";
                       ScriptStore[index]=scriptFound;
                       el.parentNode.removeChild(el);
                   }
            }
           
          }
    });
}
unloadCss() {
    return new Promise((resolve, reject) => {
        //resolve if already loaded
        var csss = document.getElementsByTagName("link");
        for (var i = 0; i < csss.length; i++) {
            var el = csss[i];
            var baseUri =el.baseURI;
            var cssFound = CssStore.find(x =>  x.name == el.id);
            if (cssFound != null)
            {
                if (el != null)
                {
                    var index = CssStore.indexOf(cssFound);
                    cssFound.loaded = false;
                    cssFound.status = "unloaded";
                    CssStore[index]=cssFound;
                    el.parentNode.removeChild(el);
                }
                  
            }
           
          }
    });
}
}