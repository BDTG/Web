// Requirement 1: Update Personal Info
document.getElementById('update-info-btn').addEventListener('click', function() {
    const currentName = document.getElementById('display-name').innerText;
    const currentMssv = document.getElementById('display-mssv').innerText;
    const currentClass = document.getElementById('display-class').innerText;

    const newName = prompt("Nhập Họ tên mới:", currentName);
    const newMssv = prompt("Nhập MSSV mới:", currentMssv);
    const newClass = prompt("Nhập Lớp mới:", currentClass);

    if (newName) document.getElementById('display-name').innerText = newName;
    if (newMssv) document.getElementById('display-mssv').innerText = newMssv;
    if (newClass) document.getElementById('display-class').innerText = newClass;
});

// Requirement 2: Add Hobby
document.getElementById('add-hobby-btn').addEventListener('click', function() {
    const newHobby = prompt("Nhập sở thích mới của bạn:");

    if (newHobby === null) return; // User cancelled

    if (newHobby.trim() === "") {
        alert("Sở thích không được để trống!");
    } else {
        const li = document.createElement('li');
        li.className = "list-group-item";
        li.innerText = newHobby;
        document.getElementById('hobby-list').appendChild(li);
    }
});

// Requirement 3: Toggle Source Code
document.getElementById('toggle-source-btn').addEventListener('click', function() {
    const sourceBlock = document.getElementById('source-code-block');
    if (sourceBlock.style.display === "none") {
        sourceBlock.style.display = "block";
        this.innerText = "Ẩn mã nguồn";
    } else {
        sourceBlock.style.display = "none";
        this.innerText = "Hiện mã nguồn";
    }
});

// Requirement 4: Dark Mode Toggle
document.getElementById('dark-mode-toggle').addEventListener('click', function() {
    document.body.classList.toggle('dark-mode');
    
    const icon = this.querySelector('i');
    if (document.body.classList.contains('dark-mode')) {
        icon.className = "fa fa-sun-o";
        this.innerHTML = '<i class="fa fa-sun-o"></i> Light Mode';
    } else {
        icon.className = "fa fa-moon-o";
        this.innerHTML = '<i class="fa fa-moon-o"></i> Dark Mode';
    }
});
