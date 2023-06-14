var LOOPS = function()
{
    let loop;
    let fps = 1;
    let gameloopTimeCount = 0;              //Room 이 준비되고 시작되었을때 1초마다 1씩 늘린다. 
    this.LogMsg = function()                //준비 되었다고 만든 함수
    {
        console.log("GAMELOOPS");
    }
    this.StartLoops = function (params , rooms , ws , room)  //루프 시작 
    {
        loop = setInterval(() => {
            gameloopTimeCount += 1;
            console.log("Looping : " + gameloopTimeCount);

            obj = {
                "type" : "info",
                "myParams" : {
                    "room" : ws["room"],
                    "loopTimeCount" : gameloopTimeCount
                }
            }       //JSON 포멧 형식

            //룸 안에 있는 모든 사람들에게 전달 
            for(var i = 0 ; i < rooms[room].length; i++)
            {
                rooms[room][i].send(JSON.stringify(obj));   //JSON 포멧으로 변환 후 Send 전송
            }            
        }, 1000/fps);       //1초마다 1번씩 Room 있는 사람들에게 전달 한다. 
    }
};

module.exports = LOOPS;