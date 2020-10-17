// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let form = document.forms["filter"];
let filterAction = document.forms["filter"]["FilterAction"];
let selectedProperty = document.getElementById("SelectedProperty");
let searchValue = document.getElementById("SearchValue");
let searchBtn = document.getElementById("search");
let cancelBtn = document.getElementById("cancel");

searchBtn.addEventListener("click", search);
cancelBtn.addEventListener("click", cancel);

function search() {
    filterAction.value = 0;
    form.submit();
}

function cancel() {
    filterAction.value = 1;
    searchValue.value = "";
    selectedProperty.selectedIndex = 0;
    form.submit();
}