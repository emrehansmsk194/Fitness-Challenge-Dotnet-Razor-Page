function showSearch(){
    if(document.getElementById("form2") != null){
        return;
    }
    var searchForm = document.createElement("form");
    searchForm.id = "form2";
    searchForm.method = "get";
    
    searchForm.innerHTML = ` 
    <div class="form-group">
            <label for="Keyword">Keywords</label>
            <input type="text" id="Keyword" name="Keyword" class="form-control" />
        </div>
        <div class="form-group">
            <label for="Category">Category</label>
            <input type="text" id="Category" name="Category" class="form-control" /> 
        </div>
        <div class="form-group">
            <label for="Difficulty">Difficulty Level</label>
            <select id="Difficulty" name="Difficulty" class="form-control">
                <option value="">All</option>
                <option value="Easy">Easy</option>
                <option value="Medium">Medium</option>
                <option value="Hard">Hard</option>
            </select>
        </div>
        <div class="form-group">
            <label for="StartDate">Start Date</label>
            <input type="date" id="StartDate" name="StartDate" class="form-control" />
        </div>
        <div class="form-group">
            <label for="EndDate">End Date</label>
            <input type="date" id="EndDate" name="EndDate" class="form-control" />
        </div>
        <button type="submit" class="btn btn-primary">Search</button>

`;
    searchForm.style.width = "50%";
    var ele = document.getElementById("table-1");
   
 
    ele.parentNode.insertBefore(searchForm,ele);


    
}