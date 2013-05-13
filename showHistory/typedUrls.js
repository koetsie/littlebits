// Copyright (c) 2012 The Chromium Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

// Event listner for clicks on links in a browser action popup.
// Open the link in a new tab of the current window.
function onAnchorClick(event) {
  chrome.tabs.create({
    selected: true,
    url: event.srcElement.href
  });
  return false;
}


function buildDomainTags(divName, domains){
    
    var popupDiv = document.getElementById(divName);
   
    var domaindiv = document.createElement('div');
        domaindiv.className="domainLinks";
    
    
        var domainStr =""
        for (var d in domains){
         var tmpstr= "<a href='#" + d + "' class='dAnchor' >" + d + "</a>&nbsp;|&nbsp;"; 
             domainStr= domainStr + " " + tmpstr;
        }
    
    
       domaindiv.innerHTML= domainStr;
    
        popupDiv.appendChild(domaindiv);
    

}


// Given an array of URLs, build a DOM list of those URLs in the
// browser action popup.
function buildPopupDom(divName,links, LinkHeader) {
  var popupDiv = document.getElementById(divName);
 
    
    var domaindiv = document.createElement('div');
        domaindiv.className="linksDiv";
    
    
    var divheader = document.createElement('div');
        divheader.className="linksheader";
        divheader.innerHTML= "<a id='" + LinkHeader + "' class='domainAnchor'>" + LinkHeader + "</a>";
    
    var ul = document.createElement('ul');
    
    
    domaindiv.appendChild(divheader);
    domaindiv.appendChild(ul);
    
    
    popupDiv.appendChild(domaindiv);
    
      
    
    

 for (var i = 0, ie = links.length; i < ie; ++i) {
     
      var l1 = links[i]
      
      
    
      var a = document.createElement('a');
          a.href = l1[0];
          a.title =a.href;
          a.className ="hlink"
     
          var a_txtnode = l1[1] ;
     
          a.appendChild(document.createTextNode(a_txtnode));
     
          a.addEventListener('click', onAnchorClick);

    
     var li = document.createElement('li');
        li.appendChild(a);
     
     
     var divdetails = document.createElement('div');
        divdetails.className="linksdetails";
        divdetails.innerHTML= "[&nbsp;visited: " + l1[5] + "&nbsp;|&nbsp;typed: " +  l1[4] + "&nbsp;|&nbsp;id: " + l1[3] + "&nbsp;]";
        li.appendChild(divdetails);

     
        ul.appendChild(li);
  }
    
}


function get_domain(data) {
    var    a      = document.createElement('a');
    var    d="";
    
    a.href = data;
    
    return a.hostname;
}



// Search history to find up to ten links that a user has typed in,
// and show those links in a popup.
function buildTypedUrlList(divName) {
  // To look for history items visited in the last week,
  // subtract a week of microseconds from the current time.
  var microsecondsPerWeek = 1000 * 60 * 60 * 24 * 7;
  var microseconsPerMonth = 1000 * 60 * 60 * 24 * 30;
    
  var oneWeekAgo = (new Date).getTime() - microsecondsPerWeek;
  var oneMonthAgo = (new Date).getTime() - microseconsPerMonth;

  // Track the number of callbacks from chrome.history.getVisits()
  // that we expect to get.  When it reaches zero, we have all results.
    
  var numRequestsOutstanding = 0;

  var mylinks =[];
  var mylinksObj=[];
  
    

  chrome.history.search({
      'text': '',               // Return every history item....
      'startTime': oneMonthAgo,  // that was accessed less than one week/month ago.
      'maxResults':400
    },
    function(historyItems) {
      // For each history item, get details on all visits.
     
     
     
                        
      for (var i = 0; i < historyItems.length; ++i) {
                       
                        
                        
                        var url = historyItems[i].url;
                        var title = historyItems[i].title;
                        var id = historyItems[i].id;
                        var typedCount  = historyItems[i].typedCount;
                        var visitCount = historyItems[i].visitCount;
                        
                        
                        // extract the domain name only
                        var domain = get_domain(url);
                        
                                               
                        var linkitem =[];
                        
                        
                        linkitem[0]= url;
                        
                        if (title == ""){
                            linkitem[1]= "no title " ;
                        }else{
                            linkitem[1]= title;
                        };
                        
                        linkitem[2]= domain;
                        linkitem[3]= id;
                        linkitem[4]= typedCount;
                        linkitem[5]= visitCount;
                        
                        
                        
                                                   
                        
                        if(!mylinksObj[domain]){
                        
                            mylinksObj[domain]=domain;
                        
                            mylinks.push(linkitem);
                        
                        }else{
                        
                            mylinks.push(linkitem);
                        }
                        
                                               
               
                    
                        
                        
                        
                        
                                     

                        
        
        var processVisitsWithUrl = function(url) {
          // We need the url of the visited item to process the visit.
          // Use a closure to bind the  url into the callback's args.
          return function(visitItems) {
            processVisits(url,visitItems);
          };
        };
        chrome.history.getVisits({url: url}, processVisitsWithUrl(url));
        numRequestsOutstanding++;
      }
      if (!numRequestsOutstanding) {
        onAllVisitsProcessed();
      }
    });


  // Maps URLs to a count of the number of times the user typed that URL into
  // the omnibox.
   // var urlToCount = {};
    
   
      
  // Callback for chrome.history.getVisits().  Counts the number of
  // times a user visited a URL by typing the address.
  var processVisits = function(url, visitItems) {
    
    
    for (var i = 0, ie = visitItems.length; i < ie; ++i) {
   
        
      //  keyword //keyword
     // if (visitItems[i].transition != 'generated') {
    //     continue;
    //  }
     
        
        
        
    }
      

    // If this is the final outstanding call to processVisits(),
    // then we have the final results.  Use them to build the list
    // of URLs to show in the popup.
    if (!--numRequestsOutstanding) {
      onAllVisitsProcessed();
    }
        
    
  };
      
      

  // This function is called when we have the final list of URls to display.
  var onAllVisitsProcessed = function() {

    
      
       buildDomainTags(divName, mylinksObj);
      
      
      for (var d in mylinksObj){
          var urlArray = [];
          var mydomain=d;
         
          for (var i = 0, ie = mylinks.length; i < ie; ++i) {
               md= mylinks[i][2];
              
              if(md==mydomain){
                   urlArray.push(mylinks[i]);
               }
          }
          
        
        
          buildPopupDom(divName, urlArray, d);
          
        };
    
    };
    
}

document.addEventListener('DOMContentLoaded', function () {
  buildTypedUrlList("typedUrl_div");
});