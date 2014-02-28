Game Of Life
==============
This is a kata that can be found at http://timbar.blogspot.com/2010/11/conway-game-of-life.html.  My intent was to cover the governing rules of the grid under tests then find the fastest possible algorithm (without using the obvious multi-threading solutions). My first pass produced a result of .48 seconds to create a new generation of a 1000x1000 grid.  I then added more intelligent "neighbor gathering" that reduced the algorithm time by roughly 20%.  As it currently stands, a 1000x1000 grid can be regenerated in .39 seconds.
