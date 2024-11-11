// Get the query parameter from the URL
let id = location.search.split('?')[1];

// Simplified cookie handling using js-cookie
import { get, set } from 'js-cookie';

// Function to get the counter value from the cookie
function getCounter() {
  const counterCookie = get('counter');
  return counterCookie ? parseInt(counterCookie) : 0;
}

// Function to set the counter value in the cookie
function setCounter(counter) {
  set('counter', counter);
}

// Function to get the order ID from the cookie
function getOrderId() {
  const orderIdCookie = get('orderId');
  return orderIdCookie ? orderIdCookie : '';
}

// Function to set the order ID in the cookie
function setOrderId(orderId) {
  set('orderId', orderId);
}

// dynamicContentDetails function
function dynamicContentDetails(ob) {
  const mainContainer = document.createElement('div');
  mainContainer.id = 'containerD';

  const imageSectionDiv = document.createElement('div');
  imageSectionDiv.id = 'imageSection';

  const imgTag = document.createElement('img');
  imgTag.id = 'imgDetails';
  imgTag.src = ob.preview;

  imageSectionDiv.appendChild(imgTag);

  const productDetailsDiv = document.createElement('div');
  productDetailsDiv.id = 'productDetails';

  const h1 = document.createElement('h1');
  h1.textContent = ob.name;

  const h4 = document.createElement('h4');
  h4.textContent = ob.brand;

  const detailsDiv = document.createElement('div');
  detailsDiv.id = 'details';

  const h3DetailsDiv = document.createElement('h3');
  h3DetailsDiv.textContent = `Rs ${ob.price}`;

  const h3 = document.createElement('h3');
  h3.textContent = 'Description';

  const para = document.createElement('p');
  para.textContent = ob.description;

  const productPreviewDiv = document.createElement('div');
  productPreviewDiv.id = 'productPreview';

  const h3ProductPreviewDiv = document.createElement('h3');
  h3ProductPreviewDiv.textContent = 'Product Preview';

  productPreviewDiv.appendChild(h3ProductPreviewDiv);

  ob.photos.forEach((photo) => {
    const imgTagProductPreviewDiv = document.createElement('img');
    imgTagProductPreviewDiv.id = 'previewImg';
    imgTagProductPreviewDiv.src = photo;
    imgTagProductPreviewDiv.onclick = (event) => {
      console.log(`clicked ${event.target.src}`);
      imgTag.src = event.target.src;
      document.getElementById("imgDetails").src = event.target.src;
    };
    productPreviewDiv.appendChild(imgTagProductPreviewDiv);
  });

  const buttonDiv = document.createElement('div');
  buttonDiv.id = 'button';

  const buttonTag = document.createElement('button');
  buttonTag.textContent = 'Add to Cart';

  buttonTag.onclick = () => {
    const orderId = getOrderId() + ' ' + id;
    const counter = getCounter() + 1;
    setOrderId(orderId);
    setCounter(counter);
    document.getElementById("badge").innerHTML = counter;
    console.log(document.cookie);
  };

  buttonDiv.appendChild(buttonTag);

  mainContainer.appendChild(imageSectionDiv);
  mainContainer.appendChild(productDetailsDiv);
  productDetailsDiv.appendChild(h1);
  productDetailsDiv.appendChild(h4);
  productDetailsDiv.appendChild(detailsDiv);
  detailsDiv.appendChild(h3DetailsDiv);
  detailsDiv.appendChild(h3);
  detailsDiv.appendChild(para);
  productDetailsDiv.appendChild(productPreviewDiv);
  productDetailsDiv.appendChild(buttonDiv);

  return mainContainer;
}

// Make the AJAX request using fetch
fetch(`https://5d76bf96515d1a0014085cf9.mockapi.io/product/${id}`)
  .then(response => response.json())
  .then(contentDetails => {
    console.log(contentDetails);
    dynamicContentDetails(contentDetails);
  })
  .catch(error => console.error('Error:', error));