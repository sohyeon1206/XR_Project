const WebSocket = require('ws');
var CREATE = require('./Create.js');

// 서버 8000번 포트 오픈
const wss = new WebSocket.Server({port:8000}, () => {
    console.log('서버 시작');
});

const userList = [];

const maxClients = 5;       // 방 최대 접속 인원수
let rooms = {};             // 룸 배열
let joinuserTemp = 1;       // 유저

wss.on('connection', function connection(ws) {
    ws.clientID = getKey(8);

    var create = new CREATE();      // 방 생성 객체를 new로 선언

    ws.on('message', (data) => {                // Message 이벤트
        const jsonData = JSON.parse(data);      // 받은 데이터를 JSON 파싱한다.

        let requestType = jsonData.requestType;
        let params = jsonData.message;

        console.log('받은 데이터 : ', jsonData, requestType, params);

        if(requestType == 10)
        {
            ws.send(JSON.stringify({userList}));
        }
        if(requestType == 100)
        {
            create.createRoom(params, rooms, ws);
        }
        if(requestType == 200)
        {
            joinRoom(params, ws);
        }
        if(requestType == 300)
        {
            leaveRoom(params);
        }
        if(requestType == 0)
        {
            wss.clients.forEach((client) => 
            {
                client.send(data);              // 접속한 클라이언트들에게 send한다.
            });
        }
    });

    ws.on('close', () => {
        const index = userList.indexOf(ws.clientID);
        if(index !== -1)
        {
            console.log('클라이언트 해제', ws.clientID);
            userList.splice(index, 1);      // 배열에서 해당 클라이언트 제거
        }
    });

    userList.push(ws.clientID);     // 새로 연결도니 클라이언트를 유저 리스트에 추가

    ws.send(JSON.stringify({clientID: ws.clientID}));       // 클라이언트에게 임시 유저 이름 전송

    console.log('클라이언트 연결 - ID', ws.clientID);
});

function generalInformation(ws)
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

function joinRoom(params, ws)
{
    const room = params;
    if(!Object.keys(rooms).includes(room))
    {
        console.warn(`Room ${room} dose net exist!`);   // 룸이 없으면 존재하지 않는 다는 메세지
        return;
    }

    if(rooms[room].length >= maxClients)        // 5명 이상 못들어가게 막는다
    {
        console.warn(`Room ${room} is full!`);
        return;
    }

    rooms[room].push(ws);       // ws 소캣을 룸에 넣는다.
    ws["room"] = room;

    generalInformation(ws);

    var UserList = "";

    for(let i = 0; i < rooms[room].length; i++)
    {
        UserList += "User : " + rooms[room][i].user + "\n";
    }
    joinuserTemp += 1;

    obj = {
        "type" : "info",
        "myParams" :
        {
            "room" : ws["room"],
            "UserList" : UserList,
        }
    }

    for(var i = 0; i < rooms[room].length; i++)
    {
        rooms[room][i].send(JSON.stringify(obj));
    }
}

function leaveRoom(params)      // 룸을 나갈경우
{
    const room = ws.room;

    if(rooms[room].length > 0)      // 룸이 존재할 때
    {
        rooms[room] = rooms[room].filter(so => so != ws);

        ws["room"] = undefined;

        if(rooms[room].length == 0)     // 룸이 0명이 되었을 때
        {
            close(room);
        }
    }

    function close(room)        // 룸 제거 함수
    {
        if(rooms.length > 0)
        rooms = rooms.filter(key => key !== room);
    }
}

wss.on('listening', () => {
    console.log('리스닝...');
});

function getKey(length) {
    let result = '';
    const characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    for(let i = 0; i < length; i++)
    {
        result += characters.charAt(Math.floor(Math.random() * characters.length));
    }

    return result;
}