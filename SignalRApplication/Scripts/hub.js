$(function () {
    var gameOfLifeHub = $.connection.gameOfLifeHub;
    var cellSize = 10;

    $.connection.hub.start().done(function () {
        gameOfLifeHub.server.getDimensions().done(function (dimensions) {
            var canvas = document.getElementById('gridCanvas');
            canvas.width = cellSize * dimensions.NumberOfColumns;
            canvas.height = cellSize * dimensions.NumberOfRows;
        });

        $("#restart").click(function () {
            gameOfLifeHub.server.restart();
        });
    });

    gameOfLifeHub.client.updateLivingCells = function (livingCells) {
        var canvas = document.getElementById('gridCanvas');
        var context = canvas.getContext('2d');
        context.clearRect(0, 0, canvas.width, canvas.height);

        $.each(livingCells, function (i) {
            context.beginPath();
            context.rect(this.X * cellSize, this.Y * cellSize, cellSize, cellSize);
            context.fillStyle = 'black';
            context.fill();
            context.lineWidth = 1;
            context.strokeStyle = 'black';
            context.stroke();
        });
    }
});