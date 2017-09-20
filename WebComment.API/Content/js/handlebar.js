/**
 * Created by Desmond on 3/17/2016.
 */
//Retrieve the template data from the HTML .
var template = $('#entry-template').html();

var context = { "name" : "Desmond", "occupation" : "developer" };

//Compile the template data into a function
var templateScript = Handlebars.compile(template);

var html = templateScript(context);
//html = 'My name is Ritesh Kumar . I am a developer.'

$(document.body).innerHTML(html);