"use strict";

var Spa = function () {
  var init = function init() {
    Spa.Model.init();
  };

  return {
    init: init
  };
}();

Spa.Data = function () {
  var configmap = {
    gametoken: null,
    env: "production"
  };

  var init = function init(gametoken, env) {
    configmap.env = env;
    configmap.gametoken = gametoken;
  };

  var players = function players() {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "https://localhost:44375/api/game/" + configmap.gametoken + "/players",
        method: "GET",
        success: function success(response) {
          resolve({
            name: "players",
            response: response
          });
        },
        error: function error(xhr, _error) {
          reject({
            name: "players",
            response: xhr
          });
        }
      });
    });
  };

  var GetGame = function GetGame() {
    //check if the board exist
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "https://localhost:44375/api/game/" + configmap.gametoken,
        method: "GET",
        success: function success(response) {
          resolve({
            name: "prep_board",
            response: response
          });
        },
        error: function error(xhr, _error2) {
          var widget = new Widget(xhr.responseText, "body", 'warning');
          widget.Load();
          reject({
            name: _error2,
            response: _error2
          });
        }
      });
    });
  };

  var pollGameStatus = function pollGameStatus() {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "https://localhost:44375/api/game/" + configmap.gametoken,
        method: "GET",
        success: function success(response) {
          resolve({
            name: "game_state",
            response: response
          });
        },
        error: function error(xhr, _error3) {
          var widget = new Widget(xhr.responseText, "body", 'warning');
          widget.Load();
          reject({
            name: "game_state",
            response: _error3
          });
        }
      });
    });
  };

  var passmove = function passmove(row, col) {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "https://localhost:44375/api/game/move",
        method: "PUT",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          "moveX": row,
          "moveY": col,
          "GameToken": configmap.gametoken.toString()
        }),
        success: function success(response) {
          resolve({
            name: "setmove",
            response: response
          });
        },
        error: function error(xhr, _error4) {
          reject({
            name: "setmove",
            response: _error4
          });
        }
      });
    });
  };

  return {
    init: init,
    GetPlayers: players,
    loadgame: GetGame,
    move: passmove
  };
}();

Spa.Model = function () {
  var init = function init() {
    //setup the token and
    var url = window.location.href.toString(),
        access_token = url.match(/\#(?:access_token)\=([\S\s]*?)\&/)[1];
    Spa.Data.init(access_token, "production");
    load();
  };

  var load = function load() {
    Spa.Data.GetPlayers().then(function (result) {
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {// var widget = new Widget(result["response"]["description"], "#widgetPlace");
        // widget.Load();
      }
    }).then(function () {
      return Spa.Data.loadgame();
    }).then(function (result) {
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var _data = JSON.parse(result['response']['data']);

        SetWhoIs(_data["OnSet"]);
        Spa.Reversi.buildgame(result["response"]["data"]);
      }
    }).then(function () {
      var poll = setInterval(function () {
        Spa.Data.loadgame().then(function (result) {
          if (result["response"]["status"] == "error") {
            var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
            widget.Load();

            if (data['GameStatus'] == "finished") {
              clearInterval(poll);
            }
          } else {
            var _data2 = JSON.parse(result['response']['data']);

            var _NewGrid = JSON.parse(_data2['Board']);

            Spa.Reversi.update(_NewGrid);
            SetWhoIs(_data2["OnSet"]);
          }
        });
      }, 1500);
    });
  };

  var RequestMove = function RequestMove(row, col) {
    Spa.Data.move(row, col).then(function (result) {
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var _NewGrid2 = JSON.parse(JSON.parse(result['response']['data'])['Board']);

        Spa.Reversi.update(_NewGrid2);
        var widget = new Widget("steen gezet, wacht op tegenstander", "#widgetPlace");
        widget.Load();
      }
    });
  };

  var SetWhoIs = function SetWhoIs(onset) {
    if (onset == 1) {
      $(".stone-block-white").show();
      $(".stone-block-white").toggleClass("rotate");
      $(".stone-block-black").hide();
    } else {
      $(".stone-block-black").toggleClass("rotate");
      $(".stone-block-white").hide();
      $(".stone-block-black").show();
    }
  };

  return {
    init: init,
    load: load,
    RequestMove: RequestMove
  };
}();

Spa.Reversi = function () {
  var config = {
    cols: 8,
    rows: 8
  };
  var CurrentGrid;
  var states = {
    'blank': {
      'id': 0,
      'color': 'white'
    },
    'white': {
      'id': 1,
      'color': 'white'
    },
    'black': {
      'id': 2,
      'color': 'black'
    }
  };

  var init = function init(grid) {
    CurrentGrid = JSON.parse(JSON.parse(grid)["Board"]);
    prepareBoard(); //check if both players are here
    // //call the api to check if the server is available
    // Reversi.model.init().then(result => {
    // 	//based on the given result
    // 	CurrentGrid = JSON.parse(result["response"]['board']);
    // 	whois(result["response"]["onSet"]);
    // 	switch (result['response']['gameStatus']) {
    // 		case "waiting":
    // 			//waiting for second player
    // 			break;
    // 		case "inprogress":
    // 			//game has started
    // 			config.started = true;
    // 			break;
    // 		case "finished":
    // 			config.finished = true;
    // 			break;
    // 			//game has finished
    // 	}
    // }).then(function() {
    // 	prepareBoard();
    // })
    // .then(function () {
    // 	if (config.finished == true) {
    // 		//game is over
    // 	} else if(config.started == false) {
    // 		//game has not started, poll the game states for our second teammate
    // 		var widget = new Widget("wachten op tegenstander", "#widgetPlace", 'warning');
    // 		widget.Load();
    // 		PollOponentStatus();
    // 	} else {
    // 		//game is in progress, keep updating
    // 		PollForGameUpdate();
    // 	}
    // })
  };

  var prepareBoard = function prepareBoard() {
    //Retrieve the board from the api
    //create the table where game takes place
    document.getElementById("board").innerHTML = "";
    var table = document.createElement("table"); // //create a square board

    for (var row = 0; row < config.rows; row++) {
      var tr = document.createElement('tr');
      table.appendChild(tr);

      for (var col = 0; col < config.cols; col++) {
        var td = document.createElement('td'); //create a stone

        var stone = document.createElement("span");
        stone.classList.add("stone");
        stone.classList.add("animate");
        var checkState = CurrentGrid[row][col];
        var state = void 0;

        switch (checkState) {
          case 0:
            stone.style.visibility = "hidden";
            state = states.blank;
            break;

          case 1:
            state = states.white;
            stone.classList.add("white");
            break;

          case 2:
            stone.classList.add("black");
            state = states.black;
        }

        tr.appendChild(td);
        td.appendChild(stone);
        CurrentGrid[row][col] = initItemState(stone, state); //bind the element to the grid

        stone.classList.toggle("animate");
        bindMove(td, row, col);
      }
    } //apend the created table to the field


    document.getElementById("board").appendChild(table);
  };

  var updateBoard = function updateBoard(NewGrid) {
    for (var row = 0; row < config.rows; row++) {
      for (var col = 0; col < config.cols; col++) {
        while (CurrentGrid[row][col].state.id != NewGrid[row][col]) {
          CurrentGrid[row][col].elem.style.visibility = "visible";

          if (NewGrid[row][col] == 1) {
            CurrentGrid[row][col].state = states.white;
            CurrentGrid[row][col].elem.classList.remove("black");
            CurrentGrid[row][col].elem.classList.add("white");
          } else if (NewGrid[row][col] == 2) {
            CurrentGrid[row][col].state = states.black;
            CurrentGrid[row][col].elem.classList.add("black");
            CurrentGrid[row][col].elem.classList.remove("white");
          }
        }
      }
    }
  };

  var PollForGameUpdate = function PollForGameUpdate() {
    setInterval(function () {
      Reversi.model.poll().then(function (result) {
        NewGrid = JSON.parse(result['response']['board']);
        whois(result["response"]["onSet"]);
      }).then(function () {
        updateBoard();
      });
    }, 1500);
  };

  var PollOponentStatus = function PollOponentStatus() {
    var widget = new Widget("tegenstander gevonden", "#widgetPlace");
    var status = setInterval(function () {
      Reversi.model.polStatus().then(function (result) {
        if (result['response'].gameStatus != "waiting") {
          //start the game
          clearInterval(status);
          NewGrid = JSON.parse(result["response"]["board"]);
          widget.Load();
          PollForGameUpdate();
        }
      });
    }, 1500);
  };

  var initItemState = function initItemState(elem, state) {
    return {
      'state': state,
      'elem': elem
    };
  };

  var bindMove = function bindMove(element, y, x) {
    element.onclick = function (event) {
      Spa.Model.RequestMove(y, x);
    };
  };

  var whois = function whois(who) {
    if (who == 1) {
      document.getElementById("whois").innerHTML = "wit is aan zet";
    } else {
      document.getElementById("whois").innerHTML = "zwart is aan zet";
    }
  };

  return {
    buildgame: init,
    update: updateBoard
  };
}();