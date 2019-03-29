var app = require('./services/appservice');
require('./controllers/root');

app.init();


//APIKey Google
//AIzaSyDIPN1WOrn8WeATWKKmFTlQLuulgqX3jO4


// pegar id do local
//https://maps.googleapis.com/maps/api/place/findplacefromtext/json?input=<NOME DO LOCAL>&inputtype=textquery&fields=place_id,name,rating,opening_hours,geometry&key=AIzaSyDIPN1WOrn8WeATWKKmFTlQLuulgqX3jO4


//procurar lugares perto
//https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=47.497912,19.040235&radius=20000&type=restaurant&key=AIzaSyDIPN1WOrn8WeATWKKmFTlQLuulgqX3jO4
//https://developers.google.com/places/web-service/supported_types


//Autocomplete
//https://developers.google.com/places/web-service/autocomplete

//Place Details
//https://developers.google.com/places/web-service/details