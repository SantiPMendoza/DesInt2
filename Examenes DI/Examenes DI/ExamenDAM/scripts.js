// Show the Add New Planet Modal
document.getElementById('add-planet-btn').addEventListener('click', () => {
    document.getElementById('add-planet-modal').classList.remove('hidden');
});

Array.from(document.getElementsByClassName('card')).forEach(element => {
    element.addEventListener('click', () => {
        document.getElementById('planet-view').classList.remove('hidden');
    });
});

// Cancel the Add New Planet Modal
document.getElementById('cancel-add').addEventListener('click', () => {
    document.getElementById('add-planet-modal').classList.add('hidden');
});

document.getElementById('close-planet-view').addEventListener('click', () => {
    document.getElementById('planet-view').classList.add('hidden');
});

// Import Data (Mock Action)
document.getElementById('import-btn').addEventListener('click', () => {
    // OpenFileDialog mock
    alert('Import Data: Select a JSON file in a real application.');
    showFeedback('Data imported successfully!', 'success');
});

// Export Data (Mock Action)
document.getElementById('export-btn').addEventListener('click', () => {
    // SaveFileDialog mock
    alert('Export Data: Save the JSON file in a real application.');
    showFeedback('Data exported successfully!', 'success');
});

document.getElementById('save-planet-data').addEventListener('click', () => {
    // SaveFileDialog mock
    alert('Export Data: Save the CSV file in a real application.');
    showFeedback('Data exported successfully!', 'success');
});


// Show Feedback Message
function showFeedback(message, type) {
    const feedback = document.getElementById('operation-feedback');
    feedback.textContent = message;
    feedback.className = `feedback ${type}`;
    feedback.classList.remove('hidden');
    setTimeout(() => feedback.classList.add('hidden'), 3000);
}

document.getElementById('switch-view-btn').addEventListener('click', () => {
    document.getElementById('main-view').classList.add('hidden');
    document.getElementById('back-to-list-btn').classList.remove('active');

    document.getElementById('planet-grid-view').classList.remove('hidden');
    document.getElementById('switch-view-btn').classList.add('active');
});

// Back to List View
document.getElementById('back-to-list-btn').addEventListener('click', () => {
    document.getElementById('planet-grid-view').classList.add('hidden');
    document.getElementById('switch-view-btn').classList.remove('active');

    document.getElementById('main-view').classList.remove('hidden');
    document.getElementById('back-to-list-btn').classList.add('active');
});