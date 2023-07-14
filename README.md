<div align="center">
  <img src="https://github.com/Malek-Mzali/Bookzone/blob/master/Bookzone/wwwroot/img/assets/logo.png" alt="Logo" width="500" heigh="500">

  <p align="center">
    This work present my final project towards a bachelor’s degree in computer science.
    <br />
    <a href="https://bookzone.somee.com/">View Demo</a>
    ·
    <a href="https://www.linkedin.com/in/malek-mzali-163b0b222/">Contact me</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project
![Home](https://github.com/Malek-Mzali/Bookzone/blob/af4e9d1ef7ae62643a11b57c08f65314c531d5db/Bookzone/wwwroot/img/assets/home.png)

Bookzone is a responsive plateform with multiple features to facilitate and optimize the distribution, management and access of digital resources.

  
Here's the features:
* Implimented a high level of security to protect digital resources and manage users roles (Administrator, Editor, Organization, Individual).
* Responsive design that render well on a variety of screen sizes. <details><img src="https://github.com/Malek-Mzali/Bookzone/blob/993e5994f697029f768b0851a03fe497cef10ac5/Bookzone/wwwroot/img/assets/phone.png?raw=true" height="10%" width="30%"></details>
* Login, Register with email verification and profile management. <details><img src="https://github.com/Malek-Mzali/Bookzone/blob/05a8272436f16692cfd4b1be86885cb70daaa0e5/Bookzone/wwwroot/img/assets/Login.gif"></details>
* Wishlist, Cart and check out system with a fully workable payment system using "Brain Tree" technology. <details><img src="https://github.com/Malek-Mzali/Bookzone/blob/9f7ea41cfcc6948d4f2f8abd30dab29f9adaa190/Bookzone/wwwroot/img/assets/Cart%20.gif"></details>
* Search with multiples filters (ex Title, Author, Theme, Isbn, ..).<details><img src="https://github.com/Malek-Mzali/Bookzone/blob/55632a432b97ed0e5c489276b4a56009e502e0d2/Bookzone/wwwroot/img/assets/search.png"></details>
* Allow users to access or download a desired chapter via the summary feature.<details><img src="https://github.com/Malek-Mzali/Bookzone/blob/661cfeb2868b6b6a28af99909275413250221186/Bookzone/wwwroot/img/assets/summary.gif"></details>
* Comment section to express your point of view of each resource.<details><img src="https://github.com/Malek-Mzali/Bookzone/blob/2a3f3ba3c021f86b32a91519a4d86a3548ca91a2/Bookzone/wwwroot/img/assets/COMMENT.png"></details>
* Dashboard 
    - [ ] General statistics to keep you in track of your plateform ex Sales and visitors overview.<details><img src="https://github.com/Malek-Mzali/Bookzone/blob/bb281d059c3809585838e69338af06689ae0c564/Bookzone/wwwroot/img/assets/stats.png"></details>
    - [ ] View/Add/Manage/Delete users
    - [ ] View/Add/Manage/Delete authors
    - [ ] View/Add/Manage/Delete themes and collections 
    - [ ] View/Add/Manage/Delete digital resources
          </br>You can import resources in a 3 different methods :
        - [x] Mnually
        - [x] Marc21 files (an integrated format defined for the identification and description of bibliographic resources)
        - [x] Google and amazon api (a search bar that allows you to look for your destinated resource)

The administrator assign collections which contains theme(s) to editor(s), the specified editor(s) will have the ability to manage only
the assigned collection(s) and their resources.

### Built With

These are the frameworks/libraries used to built this project.

* [![.Net]][.Net-url]
* [![Bootstrap.com]][Bootstrap-url]
* [![JQuery][JQuery.com]][JQuery-url]
* [![Ajax][Ajax.dev]][Ajax-url]
* [![sql][sql.com]][sql-url]


<!-- GETTING STARTED -->
## Getting Started

To get a local copy up and running follow these simple example steps.

### Installation

_Below is an example of how you can instruct your audience on installing and setting up your app. This template doesn't rely on any external dependencies or services._

1. Setup a sql server database
2. Edit appsettings.json
3. In DatabaseConfig section change DbConnnectionString to your database string example
   ```sh
   "DatabaseConfig": {
    "DbConnnectionString": "Data Source=LOUKA\\SQLEXPRESS;Initial Catalog=Work;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    },
   ```
4. Get your Api key from Braintreee and edit section
   ```sh
    "BraintreeGateway": {
    "Environment": "Your environment type ex SANDBOX",
    "MerchantId": "Merchant ID",
    "PublicKey": "Public Key",
    "PrivateKey": "Private Key"
    },
   ```
4. Edit gmail smtp informatio in this section
      ```sh
    "EmailGateway": {
      "Address": "emailAddress@gmail.com",
      "Password": "applicationPassword",
      "From": "emailAddress@gmail.com",
      "Subject": "Activate account(email title)",
      "host": "ex : https://localhost:5001/"
    },
   ```

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/othneildrew/Best-README-Template.svg?style=for-the-badge
[contributors-url]: https://github.com/othneildrew/Best-README-Template/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/othneildrew/Best-README-Template.svg?style=for-the-badge
[forks-url]: https://github.com/othneildrew/Best-README-Template/network/members
[stars-shield]: https://img.shields.io/github/stars/othneildrew/Best-README-Template.svg?style=for-the-badge
[stars-url]: https://github.com/othneildrew/Best-README-Template/stargazers
[issues-shield]: https://img.shields.io/github/issues/othneildrew/Best-README-Template.svg?style=for-the-badge
[issues-url]: https://github.com/othneildrew/Best-README-Template/issues
[license-shield]: https://img.shields.io/github/license/othneildrew/Best-README-Template.svg?style=for-the-badge
[license-url]: https://github.com/othneildrew/Best-README-Template/blob/master/LICENSE.txt
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/othneildrew
[product-screenshot]: images/screenshot.png
[.Net]: https://img.shields.io/badge/-.NET-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[.Net-url]: https://nextjs.org/](https://dotnet.microsoft.com/en-us/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Ajax.dev]: https://img.shields.io/badge/Ajax-4A4A55?style=for-the-badge&logo=ajax&logoColor=FF3E00
[Ajax-url]: https://jquery.com 
[sql.com]: https://img.shields.io/badge/Sql-FF2D20?style=for-the-badge&logo=sql&logoColor=white
[sql-url]: https://www.microsoft.com/en-us/sql-server
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com 
