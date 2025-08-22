using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SystemPropertyBackend.Data;
using SystemPropertyBackend.Models;
using Xunit;
using System.Linq;
using Assert = Xunit.Assert;
public class PropertyRepositoryTests3
{
    [Fact]
    public async Task GetFilteredAsync_ReturnsExpectedResults()
    {
        // Arrange
        var mockContext = new Mock<IMongoContext>();

        // IDs
        var idProp = ObjectId.GenerateNewId();
        var idOwner = ObjectId.GenerateNewId();

        // Datos simulados
        var sampleProperties = new List<Property>
        {
            new Property
            {
                IdProperty = idProp,
                IdOwner = idOwner,
                Name = "Casa Bonita",
                AddressP = "Calle 123",
                Price = 1000,
                Year = 2020,
                CodeInternal= "CB2020"
            }
        };

        var sampleOwners = new List<Owner>
        {
            new Owner
            {
                IdOwner = idOwner,
                Name = "Juan Pérez",
                AddressO = "Calle 123 #45-67, Bogotá",
                Photo= "juanperez.jpg",
                Birthday= new DateTime(1980, 1, 1),
            }
        };

        var sampleImages = new List<PropertyImage>
        {
            new PropertyImage
            {
                IdPropertyImage = "123",
                IdProperty = idProp,
                Enabled = true,
                File = "imagen.jpg"
            }
        };

        // Mock collections
        var mockPropertyCollection = new Mock<IMongoCollection<Property>>();
        var mockOwnerCollection = new Mock<IMongoCollection<Owner>>();
        var mockImageCollection = new Mock<IMongoCollection<PropertyImage>>();

        mockContext.Setup(c => c.Properties).Returns(mockPropertyCollection.Object);
        mockContext.Setup(c => c.Owners).Returns(mockOwnerCollection.Object);
        mockContext.Setup(c => c.PropertyImages).Returns(mockImageCollection.Object);

        // Mock Find para Property 1
        var mockCursorProperty = new Mock<IAsyncCursor<Property>>();

        mockPropertyCollection
            .Setup(c => c.FindSync(
            It.IsAny<FilterDefinition<Property>>(),
            It.IsAny<FindOptions<Property, Property>>(),
            It.IsAny<CancellationToken>()))
            .Returns(mockCursorProperty.Object);

        mockCursorProperty.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        mockCursorProperty.Setup(c => c.Current).Returns(sampleProperties);

        //mockCursorProperty
        //    .Setup(x => x.ToListAsync(It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(sampleProperties);
 
        // Mock Find para Owner
        var mockCursorOwner = new Mock<IAsyncCursor<Owner>>();

        mockOwnerCollection
            .Setup(c => c.FindSync(
            It.IsAny<FilterDefinition<Owner>>(),
            It.IsAny<FindOptions<Owner, Owner>>(),
            It.IsAny<CancellationToken>()))
            .Returns(mockCursorOwner.Object);

        mockCursorOwner.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        mockCursorOwner.Setup(c => c.Current).Returns(sampleOwners);

    
        // Mock Find para PropertyImage
        var mockCursorPropertyImage = new Mock<IAsyncCursor<PropertyImage>>();

        mockImageCollection
            .Setup(c => c.FindSync(
            It.IsAny<FilterDefinition<PropertyImage>>(),
            It.IsAny<FindOptions<PropertyImage, PropertyImage>>(),
            It.IsAny<CancellationToken>()))
            .Returns(mockCursorPropertyImage.Object);

        mockCursorPropertyImage.SetupSequence(c => c.MoveNextAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        mockCursorPropertyImage.Setup(c => c.Current).Returns(sampleImages);

        var repo = new PropertyRepository(mockContext.Object);

        // Act
        var result = new List<PropertyDto>();
        if (repo != null)
        {
            result = await repo.GetFilteredAsync(null, null, null, null);
        }
        

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        var item = result.First();
        Assert.Equal("Casa Bonita", item.Name);
        Assert.Equal("Juan Pérez", item.OwnerName);
        Assert.Equal("imagen.jpg", item.Image);
    }
}
