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

        mockCursorProperty
            .Setup(x => x.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(sampleProperties); // DEVUELVE TU LISTA
        //Console.WriteLine($"[TEST] Properties count: {properties.Count}");

        //---2
        //    mockPropertyCollection.Setup(static x => x.Find(
        //    It.IsAny<FilterDefinition<Property>>(),
        //    It.IsAny<FindOptions>()))
        //.Returns((IFindFluent<Property, Property>)Mock.Of<IAsyncCursor<Property>>());

        //    mockPropertyCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Property>>(), null))
        //        .Returns((IFindFluent<Property, Property>)Mock.Of<IAsyncCursor<Property>>());

        //    mockPropertyCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Property>>(), null)
        //            .ToListAsync(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(sampleProperties);

        // Mock cursor para propiedades 3
        //var mockPropertyCursor = new Mock<IAsyncCursor<Property>>();
        //mockPropertyCursor.Setup(_ => _.Current).Returns(sampleProperties);
        //mockPropertyCursor
        //    .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
        //    .Returns(true)
        //    .Returns(false);
        //mockPropertyCursor
        //    .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(true)
        //    .ReturnsAsync(false);

        //// Mock ToListAsync
        //mockPropertyCursor
        //    .Setup(c => c.ToListAsync(It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(sampleProperties);

        //// Mock Find para que retorne tu cursor
        //mockPropertyCollection
        //    .Setup(c => c.Find(It.IsAny<FilterDefinition<Property>>(), It.IsAny<FindOptions>()))
        //    .Returns((IFindFluent<Property, Property>)mockPropertyCursor.Object);

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

        //mockCursorOwner
        //    .Setup(x => x.ToListAsync(It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(sampleOwners); // DEVUELVE TU LISTA
        //Console.WriteLine($"[TEST] Owners count: {owners.Count}");
        //---no funciona
        //    mockOwnerCollection.Setup(x => x.Find(
        //    It.IsAny<FilterDefinition<Owner>>(),
        //    It.IsAny<FindOptions>()))
        //.Returns((IFindFluent<Owner, Owner>)Mock.Of<IAsyncCursor<Owner>>());

        //    mockOwnerCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<Owner>>(), null)
        //            .FirstOrDefaultAsync(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(sampleOwners.First());

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

        //mockCursorPropertyImage
        //    .Setup(x => x.ToListAsync(It.IsAny<CancellationToken>()))
        //    .ReturnsAsync(sampleImages); // DEVUELVE TU LISTA
        //---no funciona
        //    mockImageCollection.Setup(x => x.Find(
        //    It.IsAny<FilterDefinition<PropertyImage>>(),
        //    It.IsAny<FindOptions>()))
        //.Returns((IFindFluent<PropertyImage, PropertyImage>)Mock.Of<IAsyncCursor<PropertyImage>>());

        //    mockImageCollection.Setup(x => x.Find(It.IsAny<FilterDefinition<PropertyImage>>(), null)
        //            .FirstOrDefaultAsync(It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(sampleImages.First());
        //
        var repo = new PropertyRepository(mockContext.Object);

        // Act
        var result = new List<PropertyDto>();
        if (repo != null)
        {
            //return new List<PropertyDto>(); // o lanza una excepción si prefieres
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
