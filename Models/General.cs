namespace DigitalProject.Models;

class ErrorMessageModel {
    public string? Message { get; set; }
    public ErrorMessageModel(string? Message) {
        this.Message = Message;
    }
}