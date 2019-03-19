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
        url: "/api/game/" + configmap.gametoken + "/players",
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
        url: "/api/game/" + configmap.gametoken,
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

  var passmove = function passmove(row, col) {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "/api/game/move",
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
        error: function error(xhr, _error3) {
          reject({
            name: "setmove",
            response: _error3
          });
        }
      });
    });
  };

  var getstats = function getstats() {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "/api/stats/" + configmap.gametoken.toString(),
        method: "GET",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function success(response) {
          resolve({
            name: "stats",
            response: response
          });
        },
        error: function error(xhr, _error4) {
          reject({
            name: "stats",
            response: _error4
          });
        }
      });
    });
  };

  var skip = function skip() {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: "/api/skip/" + configmap.gametoken.toString(),
        method: "GET",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function success(response) {
          resolve({
            name: "skip",
            response: response
          });
        },
        error: function error(xhr, _error5) {
          reject({
            name: "skip",
            response: _error5
          });
        }
      });
    });
  };

  return {
    init: init,
    GetPlayers: players,
    loadgame: GetGame,
    move: passmove,
    stats: getstats,
    skip: skip
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
      //get all the players
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var widget = new Widget(result["response"]["description"], "#widgetPlace");
        widget.Load();
        $(".you-are > span").html("Jij bent " + result["response"]["data"]);
      }
    }).then(function () {
      //request the game status
      return Spa.Data.loadgame();
    }).then(function (result) {
      //build the game
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var data = JSON.parse(result['response']['data']);
        SetWhoIs(data["OnSet"]);
        Spa.Reversi.buildgame(result["response"]["data"]);
      }
    }).then(function () {
      //keep polling the game
      var poll = setInterval(function () {
        Spa.Data.loadgame().then(function (result) {
          var data = JSON.parse(result['response']['data']);

          if (result["response"]["status"] == "error") {
            var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning", false);
            widget.Load();

            if (data['GameStatus'] == "finished") {
              clearInterval(poll);
              setInterval(function () {
                location.href = "/dashboard";
              }, 5000);
            }
          } else {
            var NewGrid = JSON.parse(data['Board']);
            Spa.Reversi.update(NewGrid);
            SetWhoIs(data["OnSet"]);
          }
        });
      }, 1500);
    }).then(function () {
      //request weathers stats
      return Spa.Api.weather();
    }).then(function (result) {
      //build weather api stats
      $("#advertisement > .place-title").html(result["name"]);
      $("#advertisement > .temp").html(parseInt(result["main"]["temp"] - 273.15) + "&#8451;");
    }).then(function () {
      //get the stats for the chart
      Spa.Data.stats().then(function (result) {
        Spa.Grafiek.init(JSON.parse(result["response"]["data"]));
        Spa.Grafiek.toonGrafiek();
      });
    });
  };

  var RequestMove = function RequestMove(row, col) {
    Spa.Data.move(row, col).then(function (result) {
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var data = JSON.parse(result['response']['data']);
        var NewGrid = JSON.parse(result['response']["board"]);
        Spa.Reversi.update(NewGrid, data["MoveX"], data["MoveY"]);
        Spa.Data.stats().then(function (result) {
          Spa.Grafiek.init(JSON.parse(result["response"]["data"]));
          Spa.Grafiek.update();
        });
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
    rows: 8,
    movex: null,
    movey: null
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

  var init = function init(grid, x, y) {
    CurrentGrid = JSON.parse(JSON.parse(grid)["Board"]);
    prepareBoard();
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

  var updateBoard = function updateBoard(NewGrid, setx, sety) {
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

          if (row == setx && col == sety) {// do nothing
          } else {
            if (NewGrid[row][col] == 1) {
              CurrentGrid[row][col].elem.classList.add('animate-white');
            } else if (NewGrid[row][col] == 2) {
              CurrentGrid[row][col].elem.classList.add('animate-black');
            }
          }
        }
      }
    }
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

  var skipturn = function skipturn() {
    Spa.Data.skip().then(function (result) {
      if (result["response"]["status"] == "error") {
        var widget = new Widget(result["response"]["description"], "#widgetPlace", "warning");
        widget.Load();
      } else {
        var widget = new Widget(result["response"]["description"], "#widgetPlace");
        widget.Load();
      }
    });
  };

  return {
    buildgame: init,
    update: updateBoard,
    skip: skipturn
  };
}();

Spa.Api = function () {
  var config = {
    key: '6f88e00d35ba36cb983f840f5d1b75db',
    url: 'https://api.openweathermap.org/data/2.5/weather',
    place: 'Zwolle'
  };

  var init = function init(place) {
    config.place = place;
  };

  var GetWeather = function GetWeather() {
    return new Promise(function (resolve, reject) {
      $.ajax({
        url: config.url + '?q=' + config.place + '&appid=' + config.key,
        method: "get",
        success: function success(response) {
          resolve(response);
        }
      });
    });
  };

  return {
    init: init,
    weather: GetWeather
  };
}();

Spa.Grafiek = function () {
  var jsondata;
  var chart;

  var init = function init(data) {
    jsondata = data;
  };

  var showGrafiek = function showGrafiek() {
    var ctx = document.getElementById('chart_stats').getContext('2d');
    var myChart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['wit', 'zwart'],
        datasets: [{
          label: 'aantal fiches',
          data: [jsondata['PlayerWhiteScore'], jsondata['PlayerBlackScore']],
          backgroundColor: ['rgba(239, 237, 237, 1)', 'rgba(53, 51, 51, 1)'],
          borderColor: ['rgba(255, 99, 132, 0.2)', 'rgba(54, 162, 235, 0.2)'],
          borderWidth: 1
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        scales: {
          yAxes: [{
            ticks: {
              beginAtZero: true
            }
          }]
        }
      }
    });
    chart = myChart;
  };

  var update = function update() {
    chart.destroy();
    showGrafiek();
  };

  return {
    init: init,
    toonGrafiek: showGrafiek,
    update: update
  };
}();

Spa.Template = function () {
  var init = function init(template) {};

  var getTemplate = function getTemplate() {};

  var parseTemplate = function parseTemplate() {};

  return {
    init: init
  };
}();