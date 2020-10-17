// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let form = document.forms["filter"];
let formAction = document.forms["filter"]["FormAction"];
let selectedProperty = document.getElementById("SelectedProperty");
let searchValue = document.getElementById("SearchValue");
let firstRangeValue = document.getElementById("FirstRangeValue");
let secondRangeValue = document.getElementById("SecondRangeValue");
let searchBtn = document.getElementById("search");
let cancelBtn = document.getElementById("cancel");

let simpleValueBlock = document.getElementById("simpleValue");
let rangeValuesBlock = document.getElementById("rangeValues");

searchBtn.addEventListener("click", search);
cancelBtn.addEventListener("click", cancel);
selectedProperty.addEventListener("change", selectProperty);

function search() {
    formAction.value = 0;
    form.submit();
}

function cancel() {
    formAction.value = 1;
    searchValue.value = "";
    firstRangeValue.value = "";
    secondRangeValue.value = "";
    selectedProperty.selectedIndex = 0;
    form.submit();
}

function selectProperty() {
    if (selectedProperty.value === "Created") {
        simpleValueBlock.classList.add("d-none");
        rangeValuesBlock.classList.remove("d-none");
    } else {
        simpleValueBlock.classList.remove("d-none");
        rangeValuesBlock.classList.add("d-none");
    }
}