class Widget {
    constructor(message, element, type) {
        this.message = message;
        this.element = element;
        this.type = type;
        this.color = '#DFF2BF';
    }
    
    DestroyPrevious() {
        $(this.element).empty();
    }

    Build() {
        $(this.element)
            .append($("<div>").attr('id', 'widget').addClass('fade-in')
                .append($("<div>").addClass('widget-exit')
                    .append($("<i>").addClass('fa fa-window-close')))
                .append($("<div>").addClass('widget-title').html())
                .append($("<div>").addClass('widget-content').html(this.message))
                .append($("<div>").addClass('button-row')
                    .append($("<button>").html('akoord').addClass('btn').attr('id', 'ok').addClass('shake'))
                    .append($("<button>").html('Weigeren').addClass('btn').attr('id','cancel'))
                )
            );
    }

    AddEventListeners() {
        let place = this.element;
        $(".widget-exit").on('click', function () {
            $(place).empty();
        });
        $("#ok").on('click', function () {
            $(place).empty();
        });
        $("#cancel").on('click', function () {
            $(place).empty();
        });
    }

    SetType() {
        if (this.type === "warning") {
            this.color = "#f2dede";
            $("#widget").css('background', this.color)
            $(".button-row").hide();
        } else {
            this.color = "#DFF2BF";
            $("#widget").css('background', this.color)
        }
    }

    LogToStorage() {
        //get the items from localstorage
        let log = JSON.parse(localStorage.getItem("logs"));
        let logs = new Array();
        if (log === null) {
            //New array
            log = new Array();
            log.push(this.message);
        } else {
            //update current array
            if (log.length > 9) {
                log.shift();
                log.push(this.message);
            } else {
                log.push(this.message);
            }
        }
        localStorage["logs"] = JSON.stringify(log);
    }

    Load() {
        this.DestroyPrevious();
        this.Build();
        this.AddEventListeners();
        this.SetType();
        this.LogToStorage();
    }
}