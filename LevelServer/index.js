//We use express, express-session,express-fileupload, long and mysql

var express = require('express');
var Long = require("long");

const fileUpload = require('express-fileupload');
var app = express(); // here I use the express() method, instead of the createServer()

var con = mysql.createConnection({
  host: "localhost",
  user: "user2",
  password: "schmonzes"
});

con.connect(function(err) {
  if (err) throw err;
  console.log("Connected!");
});



app.use(fileUpload());

app.get('/', function(req, res){
  res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
  res.write("<h1>Willkommen auf dem Dungeons & Dungeons Level Finder</h1>");
  res.write("<a href='/list'>Das große Archiv</a>");
  res.end();
});
app.get('/upload', function(req,res){
 res.write('<html>');
 res.write(' <body>');
   res.write(' <form ref="uploadForm"'); 
      res.write('id="uploadForm"');
      res.write('action="/upl"'); 
      res.write('method="post"'); 
      res.write('encType="multipart/form-data"><input type="file" name="level" />');
      res.write('<input type="submit" value="Upload!" /></form></body>');
res.write('</html>');
res.end();
});
app.post('/upl', function(req, res){
  //Hier upload processen
  if (!req.files || Object.keys(req.files).length === 0) {
    return res.status(400).send('No files were uploaded.');
  }
 let levelFile = req.files.level;
 var luid = new Long(0xFFFFFFFF, 0x7FFFFFFF);

  levelFile.mv(__dirname+'/levels/'+luid.toString()+'.lev',function(err) {
  if(err) return res.status(500).send(err);
  res.send('Level hochgeladen'); });
});

app.get('/pull', function(req, res){
  //Hier download processen
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

client.query('SELECT * FROM level', (err, resp) => {  
for(i = 0;i<resp.rowCount;i++)
{
res.write('<tr><td>'+i+'</td><td>Test</td> <td>'+resp.rows[0].name+'</td></tr>');
}
res.write('</table>');
})
});
var server = app.listen(13337, function() {
  console.log('Listening on port %d', server.address().port);
});
