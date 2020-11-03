//We use express, express-session,express-fileupload, long and mysql

var express = require('express');
var Long = require("long");

const fileUpload = require('express-fileupload');
var app = express(); // here I use the express() method, instead of the createServer()

const session = require('express-session');
var mysql = require("mysql");

var con = mysql.createConnection({
  host: "localhost",
  user: "user2",
  password: "schmonzes",
  database: "LevelServer"
});

con.connect(function(err) {
  if (err) throw err;
  console.log("Connected!");
});




app.use(fileUpload());

app.get('/gameServerLoad', function(req, res){
  
  var sql = "SELECT * FROM LevelMetaData;";
  var ulid = 0;
  con.query(sql, function (err, result, fields) {
  res.write(JSON.stringify(result));
  res.end();
  });
  

});
app.get('/', function(req, res){
  res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
  
res.write('<html> \n <head>\n <title>D + D - Startseite</title> \n </head> \n');
res.write('<body>\n ');
  
  res.write("<h1>Willkommen auf dem Dungeons & Dungeons Level Finder</h1>");
  res.write("<a href='/list'>Das groÃŸe Archiv</a>");
  
res.write('</body>\n ');
res.write('</html>\n ');
  res.end();
});
app.get('/upload', function(req,res){
 res.write('<html>');
 res.write(' <body>');
   res.write(' <form ref="uploadForm"'); 
      res.write('id="uploadForm"');
      res.write('action="/upl"'); 
      res.write('method="post"'); 
      res.write('encType="multipart/form-data">');
      res.write('<table><tr> <td> File: </td>  <td> <input type="file" name="level" /></td> </tr>');
      res.write('<tr> <td> Name: </td>  <td><input type="text" name="name" /></td> </tr>');
      res.write('<tr> <td> Level Status : <b> in Ordnung </b>   </td> <td><input type="submit" value="Upload!" /></td>  </tr> </table> </form></body>');
res.write('</html>');
res.end();
});
app.post('/upl', function(req, res){
  //Hier upload processen
  if (!req.files || Object.keys(req.files).length === 0) {
    return res.status(400).send('No files were uploaded.');
  }
 let levelFile = req.files.level;

  let name = req.body.name;
  
  var sql = "INSERT INTO LevelMetaData (name,description, creationdate) VALUES ('"+name+"','Moin Servus Moin!',NOW()  );";
  console.log(sql);
  var ulid = 0;
  con.query(sql, function (err, result) {
    if (err) throw err;
    ulid = result.insertId;
  
  if(levelFile!=null)
  {
  levelFile.mv(__dirname+'/levels/'+ulid.toString()+'.lev',function(err) {
  if(err) return res.status(500).send(err); });
  }else return res.status(500).send("Form error"); 
  res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
  res.write("Level successfully uploaded!<br>");
  res.write("<table>");
  res.write("<tr> <td> Name: </td><td>"+name+"</td> </tr>");
  res.write("<tr> <td> ULID: </td><td> <b>"+ulid+"</b> </td></tr>");
    
  res.write("</table>");
  res.end();
  });
  
  
});

app.get('/pull', function(req, res){
  
  var ulid = req.query.ulid;
  //Hier download processen
  const file = __dirname+'/levels/'+ ulid +'.lev';
  res.download(file);
});

app.get('/show', function(req,res){
  var ulid = req.query.ulid;
  res.write(ulid);
  res.end();
});

app.get('/list', function(req,res){
res.writeHeader(200 , {"Content-Type" : "text/html; charset=utf-8"});
res.write('<html> \n <head>\n <title>D + D - Das groÃŸe Archiv</title> \n </head> \n');
res.write('<body>\n ');
res.write('<h1>Das groÃŸe Archiv</h1>\n ');
res.write("<a href='/'>Start</a>\n ");
res.write('<table style="width:100%">\n ');
var i;

res.write('<tr><th>ID</th><th>Name</th><th>Tags</th><th>Created on</th></tr>\n ');
  
  var sql = "SELECT * FROM LevelMetaData;";
  var ulid = 0;
  con.query(sql, function (err, result, fields) {
    if (err) throw err;
    for(i = 0;i<result.length;i++)
    {
    res.write('<tr><td>'+result[i].ULId+'</td><td> <a href="/show?ulid='+result[i].ULId+'" > '+result[i].Name+'</a> </td> <td> #Test #Cool </td><td> ' +result[i].CreationDate+ ' </td></tr>\n ');
    }
    
  res.write('</table>\n </body>\n </html>\n ');
  res.end();
  });
  
});
var server = app.listen(13337, function() {
  console.log('Listening on port %d', server.address().port);
});
