$(function () {
    var gameOfLife = $.connection.gameOfLifeHub;
    gameOfLife.client.updateLivingCells = function (livingCells) {
        renderLivingCells(livingCells);
    };

    $.connection.hub.start().done(function () {
        $("#restart").click(function () {
            gameOfLife.server.restart();
        });
    });

    function renderLivingCells(livingCells) {
        var canvas = document.getElementById('gridCanvas');
        var context = canvas.getContext('2d');
        context.clearRect(0, 0, canvas.width, canvas.height);

        $.each(livingCells, function (i) {
            context.beginPath();
            context.rect(this.X * 10, this.Y * 10, 10, 10);
            context.fillStyle = 'black';
            context.fill();
            context.lineWidth = 1;
            context.strokeStyle = 'black';
            context.stroke();
        });
    }
});