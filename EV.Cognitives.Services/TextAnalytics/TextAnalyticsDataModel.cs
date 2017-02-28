// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EV.Cognitives.Services.TextAnalytics
{
    #region Detected Language 
    public class DetectedLanguageDataModel
    {
        public List<Document> documents { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public string text { get; set; }
        public string language { get; set; }

        public double score { get; set; }
    }
    #endregion
    #region result analyze

    public class ResultAnalyzeDataModel
    {
        public List<AnalyzedDocument> documents { get; set; }
        public List<Error> errors { get; set; }
    }

    public class AnalyzedDocument
    {
        public string id { get; set; }
        public List<Detectedlanguage> detectedLanguages { get; set; }
    }

    public class Detectedlanguage
    {
        public string name { get; set; }
        public string iso6391Name { get; set; }
        public float score { get; set; }
    }

    public class Error
    {
        public string id { get; set; }
        public string message { get; set; }
    }

    #endregion

    

    

}
