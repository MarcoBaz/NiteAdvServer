export interface Script {
    name: string;
    src: string;
    loaded:boolean;
    status:string;
}  


export const ScriptStore: Script[] = [
    {name: 'easing', src:"assets/site/js/jquery.easing.js",loaded:false,status:"Unloaded"},
    {name: 'validate', src:"assets/site/js/jquery.validate.js",loaded:false,status:"Unloaded"},
    {name: 'smartWizard', src:"assets/site/js/jquery.smartWizard.js",loaded:false,status:"Unloaded"},
    {name: 'feather', src:"assets/site/js/feather.js",loaded:false,status:"Unloaded"},
    {name: 'common', src:"assets/site/js/common.js",loaded:false,status:"Unloaded"},
{name: 'counterup2', src:"assets/site/js/counterup2.js",loaded:false,status:"Unloaded"},
{name: 'noise', src:"assets/site/js/noise.js",loaded:false,status:"Unloaded"},
{name: 'noframework', src:"assets/site/js/noframework.waypoints.js",loaded:false,status:"Unloaded"},
{name: 'odometer', src:"assets/site/js/odometer.js",loaded:false,status:"Unloaded"},
{name: 'prism', src:"assets/site/js/prism.js",loaded:false,status:"Unloaded"},
{name: 'simplebar', src:"assets/site/js/simplebar.js",loaded:false,status:"Unloaded"},
{name: 'swiper', src:"assets/site/js/swiper-bundle.js",loaded:false,status:"Unloaded"},
{name: 'aos', src:"assets/site/js/aos.js",loaded:false,status:"Unloaded"},
{name: 'typed', src:"assets/site/js/typed.js",loaded:false,status:"Unloaded"},
{name: 'magnific-popup', src:"assets/site/js/jquery.magnific-popup.js",loaded:false,status:"Unloaded"},
{name: 'cookieconsent', src:"assets/site/js/cookieconsent.js",loaded:false,status:"Unloaded"},
{name: 'animatebar', src:"assets/site/js/jquery.animatebar.js",loaded:false,status:"Unloaded"},
{name: 'stripe-bubbles', src:"assets/site/js/stripe-bubbles.js",loaded:false,status:"Unloaded"},
{name: 'stripe-menu', src:"assets/site/js/stripe-menu.js",loaded:false,status:"Unloaded"},
{name: 'svg', src:"assets/site/js/svg.js",loaded:false,status:"Unloaded"},
{name: 'site', src:"assets/site/js/site.js",loaded:false,status:"Unloaded"},
{name: 'wizards', src:"assets/site/js/wizards.js",loaded:false,status:"Unloaded"}
];

/*
{name: 'filepicker', src: 'https://api.filestackapi.com/filestack.js'},
    {name: 'rangeSlider', src: '../../../assets/assets/site/js/ion.rangeSlider.min.js'},

    {name: 'jquery.easing.', src:"assets/site/js/jquery.easing.js"},
{name: 'filepicker', src:"assets/site/js/jquery.validate.js"},
{name: 'filepicker', src:"assets/site/js/jquery.smartWizard.js"},
{name: 'filepicker', src:"assets/site/js/feather.js"},
{name: 'filepicker', src:"assets/site/js/common.js"},
{name: 'filepicker', src:"assets/site/js/forms.js"},
{name: 'credit-card', src:"assets/site/js/credit-card.js"},
{name: 'pricing', src:"assets/site/js/pricing.js"},
{name: 'filepicker', src:"assets/site/js/shop.js"},
  {name: 'jquery', src: "assets/site/js/jquery.js"},
    {name: 'bootstrap', src: "assets/site/js/bootstrap.bundle.js"},
*/