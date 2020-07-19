var express = require('express');
var app = express(); // here I use the express() method, instead of the createServer()
app.get('/', function(req, res){
  res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
  res.write("<h1>Willkommen auf dem Dungeons & Dungeons Level Finder</h1>");
  res.write("<a href='/list'>Das große Archiv</a>");
  res.end();
});
app.get('/upload', function(req, res){
  //Hier upload processen
  res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
  res.write("<h1>Level hochgeladen!</h1>");
  res.write("ulid: 0");
  res.end();
});

app.get('/pull', function(req, res){
  //Hier upload processen
  console.log('Test');
  const file = `${__dirname}/levels/0000000000000000.lev`;
  res.download(file);
});
app.get('/list', function(req,res){
res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
res.write('<h1>Das große Archiv</h1>');
res.write("<a href='/'>Start</a>");
res.write('<table style="width:100%">');
var i;
res.write('<tr><th>ID</th><th>Name</th><th>Tags</th></tr>');

for(i=0; i <16;i++)
{
res.write('<tr><td>'+i+'</td><td>Ein Level</td><td>Dungeon</td></tr>');
}

res.write('</table>');
res.end();
});
var server = app.listen(3000, function() {
  console.log('Listening on port %d', server.address().port);
});
