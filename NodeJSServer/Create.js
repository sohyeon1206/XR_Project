var LOOPS = require('./Loop.js');

var CREATE = function()
{
    console.log("Create read Info ...");        // 방이 만들어졌을 때 console.log로 알려준다.
};

CREATE.prototype.LogMsg = function()
{
    console.log("Create Connect ...");      // 방 메세지
};

CREATE.prototype.generalInformation = function(ws, rooms)
{
    let obj;
    if(ws["room"] != undefined)     // ws 배열이 방에 있을 경우 진입
    {
        obj = {
            "type" : "info",
            "myParams" :
            {
                "room" : ws["room"],
                "no-clients" : rooms[ws["room"]].length,
            }
        }
    }
    else        // 방이 없다
    {
        obj = {
            "type" : "info",
            "myParams" :
            {
                "room" : "no room",
            }
        }
    }

    ws.send(JSON.stringify(obj));       // 클라이언트에 전달해준다.
}

CREATE.prototype.createRoom = function(params, rooms, ws)
{
    const room = this.getKey(5);        // 랜덤으로 방 이름을 지정해주는 함수
    console.log("room id : " + room);
    rooms[room] = [ws];
    ws["room"] = room;

    this.generalInformation(ws, rooms);

    var loops = new LOOPS();        // 방이 만들어지는 것을 확인한 후에 시간 설정
    loops.StartLoops(params, rooms, ws, room);      // 해당 루프를 실행 시킨다.
};

CREATE.prototype.getKey = function(length) {        // 랜덤으로 방 이름을 지정
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    for(let i = 0; i < length; i++)
    {
        result += characters.charAt(Math.floor(Math.random() * characters.length));
    }

    return result;
};

module.exports = CREATE;