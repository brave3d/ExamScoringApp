const droppable = new Draggable.Droppable(document.querySelectorAll("ul"), {
  draggable: "li",
  droppable: "ul"
});

droppable.on("droppable:over", function() {
  $("ul").removeClass("draggable-droppable--occupied");
});
droppable.on("droppable:out", () => console.log("droppable:out"));
