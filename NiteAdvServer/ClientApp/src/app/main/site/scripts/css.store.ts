export interface Css {
    name: string;
    src: string;
    loaded:boolean;
    status:string;
}  


export const CssStore: Css[] = [
    {name: 'fontawesome', src:"assets/site/css/fontawesome.css",loaded:false,status:"Unloaded"},
    {name: 'aos', src:"assets/site/css/aos.css",loaded:false,status:"Unloaded"},
    {name: 'cookieconsent', src:"assets/site/css/cookieconsent.min.css",loaded:false,status:"Unloaded"},
    {name: 'magnific-popup', src:"assets/site/css/magnific-popup.css",loaded:false,status:"Unloaded"},
    {name: 'odometer', src:"assets/site/css/odometer-theme-minimal.css",loaded:false,status:"Unloaded"},
    {name: 'prism-okaidia', src:"assets/site/css/prism-okaidia.css",loaded:false,status:"Unloaded"},
    {name: 'simplebar', src:"assets/site/css/simplebar.css",loaded:false,status:"Unloaded"},
    {name: 'smart_wizard_all', src:"assets/site/css/smart_wizard_all.css",loaded:false,status:"Unloaded"},
    {name: 'swiper-bundle', src:"assets/site/css/swiper-bundle.css",loaded:false,status:"Unloaded"},
    {name: 'dashcore', src:"assets/site/css/dashcore.css",loaded:false,status:"Unloaded"},
    {name: 'rtl', src:"assets/site/css/rtl.css",loaded:false,status:"Unloaded"},
    {name: 'demo', src:"assets/site/css/demo.css",loaded:false,status:"Unloaded"},
];