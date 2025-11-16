namespace ProductsStore.WebAPI.DTO.Product
{
    public record ProductAttributesDto
    (
        Guid Id,
        string? Title,
        string? Content
    );
}
