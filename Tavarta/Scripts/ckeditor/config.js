/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function (config) {





   
    config.skin = 'office2013';
    config.extraPlugins = 'colorbutton';
    config.extraPlugins = 'panel';
    config.extraPlugins = 'button';
    config.extraPlugins = 'floatpanel';
    config.extraPlugins = 'widget';
    config.extraPlugins = 'image2';
    config.extraPlugins = 'clipboard';
    config.extraPlugins = 'widgetbootstrap';
    config.extraPlugins = 'lineutils';
    config.extraPlugins = 'dialog';
    config.extraPlugins = 'panelbutton';

    config.plugin = 'wysiwygarea,toolbar,basicstyles,menubutton,link,sourcearea';
         
 

    //config.extraPlugins = 'niftyimages';
    var roxyFileman = '/Scripts/fileman/index.html?integration=ckeditor';
config.filebrowserBrowseUrl = roxyFileman;
config.filebrowserImageBrowseUrl = roxyFileman + '&type=image';
config.removeDialogTabs = 'link:upload;image:upload';
    // config.filebrowserBrowseUrl = "~/App_Data/Uploads";
    //config.filebrowserWindowWidth = 500;
    //config.filebrowserWindowHeight = 650;
    //config.filebrowserUploadUrl = "~/App_Data/Uploads";

	// Define changes to default configuration here. For example:
   config.language = 'fa';

	// config.uiColor = '#AADC6E';
};
