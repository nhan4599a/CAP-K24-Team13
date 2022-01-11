if (self.CavalryLogger) { CavalryLogger.start_js(["uu4IRyy"]); }

__d("MosUtils",[],(function(a,b,c,d,e,f){"use strict";function a(a,b){if(a===0||b===0)return 0;if(a<b){var c=a;a=b;b=c}c=a/b;return c>16/9?Math.round(a/(16/9)):b}function b(a,b){var c=null,d=null,e=null,f=null;for(var g=0;g<a.length;g++){var h=a[g].qualityLabel;if(h<=b)e=a[g].mosValue,c=h;else{f=a[g].mosValue;d=h;break}}if(c===null&&d===null)return 0;else if(c===null&&f!==null)return f;else if(d===null&&e!==null)return e;else if(f!==null&&e!==null&&c!==null&&d!==null){h=e+(b-c)*(f-e)/(d-c);return h<0?0:h>100?100:h}return 0}function c(a){a=a.split(",");var b=[];for(var c=0;c<a.length;c++){var d=a[c].split(":");if(d.length!==2)return null;var e=Number(d[0]);d=Number(d[1]);if(isNaN(e)||isNaN(d))return null;b.push({qualityLabel:e,mosValue:d})}return b}f.getQualityLabel=a;f.getMosValue=b;f.parsePlaybackMos=c}),66);
__d("DGWConstants",["$InternalEnum"],(function(a,b,c,d,e,f){"use strict";var g=3e4,h="prod",i=128,j=(b=b("$InternalEnum"))({NORMAL_CLOSURE:1e3,GOING_AWAY:1001,ABNORMAL_CLOSURE:1006,SERVER_INTERNAL_ERROR:1011,GRACEFUL_CLOSE:4e3,KEEPALIVE_TIMEOUT:4001,DGW_SERVER_ERROR:4002,UNAUTHORIZED:4003,REJECTED:4004,BAD_REQUEST:4005}),k=b({DrainReason_ELB:0,DrainReason_SLB:1,DrainReason_AppServerPush:2,DrainReason_GracePeriodExpired:3,DrainReason_Unknown:4});function a(a){switch(a){case k.DrainReason_ELB:return"DrainReason_ELB";case k.DrainReason_SLB:return"DrainReason_SLB";case k.DrainReason_AppServerPush:return"DrainReason_AppServerPush";case k.DrainReason_GracePeriodExpired:return"DrainReason_GracePeriodExpired";case k.DrainReason_Unknown:return"DrainReason_Unknown"}}var l=b({DGWVER_GENESIS:0,DGWVER_PREFIXED_APP_HEADERS:1,DGWVER_HANDLES_DGW_DRAIN_FRAME:2,DGWVER_HANDLES_DGW_DEAUTH_FRAME:3,DGWVER_HANDLES_STREAMGROUPS:4,DGWVER_BIG_IDS:5});function c(a){switch(a){case l.DGWVER_GENESIS:return"DGWVER_GENESIS";case l.DGWVER_PREFIXED_APP_HEADERS:return"DGWVER_PREFIXED_APP_HEADERS";case l.DGWVER_HANDLES_DGW_DRAIN_FRAME:return"DGWVER_HANDLES_DGW_DRAIN_FRAME";case l.DGWVER_HANDLES_DGW_DEAUTH_FRAME:return"DGWVER_HANDLES_DGW_DEAUTH_FRAME";case l.DGWVER_HANDLES_STREAMGROUPS:return"DGWVER_HANDLES_STREAMGROUPS";case l.DGWVER_BIG_IDS:return"DGWVER_BIG_IDS"}}var m=b({DgwCodecReturnCode_Success:0,DgwCodecReturnCode_Failure:1,DgwCodecReturnCode_NotEnoughData:2,DgwCodecReturnCode_OutOfMemory:3,DgwCodecReturnCode_AckIdOutOfBounds:4,DgwCodecReturnCode_InvalidParameter:5,DgwCodecReturnCode_InvalidFrameType:6,DgwCodecReturnCode_InvalidAckFrameLength:7,DgwCodecReturnCode_InvalidDrainReason:8,DgwCodecReturnCode_InvalidDataFrameLength:9,DgwCodecReturnCode_InvalidDrainFrameLength:10});function d(a){switch(a){case m.DgwCodecReturnCode_Success:return"DgwCodecReturnCode_Success";case m.DgwCodecReturnCode_Failure:return"DgwCodecReturnCode_Failure";case m.DgwCodecReturnCode_NotEnoughData:return"DgwCodecReturnCode_NotEnoughData";case m.DgwCodecReturnCode_OutOfMemory:return"DgwCodecReturnCode_OutOfMemory";case m.DgwCodecReturnCode_AckIdOutOfBounds:return"DgwCodecReturnCode_AckIdOutOfBounds";case m.DgwCodecReturnCode_InvalidParameter:return"DgwCodecReturnCode_InvalidParameter";case m.DgwCodecReturnCode_InvalidFrameType:return"DgwCodecReturnCode_InvalidFrameType";case m.DgwCodecReturnCode_InvalidAckFrameLength:return"DgwCodecReturnCode_InvalidAckFrameLength";case m.DgwCodecReturnCode_InvalidDrainReason:return"DgwCodecReturnCode_InvalidDrainReason";case m.DgwCodecReturnCode_InvalidDataFrameLength:return"DgwCodecReturnCode_InvalidDataFrameLength";case m.DgwCodecReturnCode_InvalidDrainFrameLength:return"DgwCodecReturnCode_InvalidDrainFrameLength"}}var n=b({DgwFrameType_Data:0,DgwFrameType_SmallAck:1,DgwFrameType_Empty:2,DgwFrameType_Drain:3,DgwFrameType_Deauth:4,DgwFrameType_StreamGroup_DeprecatedEstabStream:5,DgwFrameType_StreamGroup_DeprecatedData:6,DgwFrameType_StreamGroup_SmallAck:7,DgwFrameType_StreamGroup_DeprecatedEndOfData:8,DgwFrameType_Ping:9,DgwFrameType_Pong:10,DgwFrameType_StreamGroup_Ack:12,DgwFrameType_StreamGroup_Data:13,DgwFrameType_StreamGroup_EndOfData:14,DgwFrameType_StreamGroup_EstabStream:15});function e(a){switch(a){case n.DgwFrameType_Data:return"DgwFrameType_Data";case n.DgwFrameType_SmallAck:return"DgwFrameType_SmallAck";case n.DgwFrameType_Empty:return"DgwFrameType_Empty";case n.DgwFrameType_Drain:return"DgwFrameType_Drain";case n.DgwFrameType_Deauth:return"DgwFrameType_Deauth";case n.DgwFrameType_StreamGroup_DeprecatedEstabStream:return"DgwFrameType_StreamGroup_DeprecatedEstabStream";case n.DgwFrameType_StreamGroup_DeprecatedData:return"DgwFrameType_StreamGroup_DeprecatedData";case n.DgwFrameType_StreamGroup_SmallAck:return"DgwFrameType_StreamGroup_SmallAck";case n.DgwFrameType_StreamGroup_DeprecatedEndOfData:return"DgwFrameType_StreamGroup_DeprecatedEndOfData";case n.DgwFrameType_Ping:return"DgwFrameType_Ping";case n.DgwFrameType_Pong:return"DgwFrameType_Pong";case n.DgwFrameType_StreamGroup_Ack:return"DgwFrameType_StreamGroup_Ack";case n.DgwFrameType_StreamGroup_Data:return"DgwFrameType_StreamGroup_Data";case n.DgwFrameType_StreamGroup_EndOfData:return"DgwFrameType_StreamGroup_EndOfData";case n.DgwFrameType_StreamGroup_EstabStream:return"DgwFrameType_StreamGroup_EstabStream"}}b={HEADER_APPID:"x-dgw-appid",HEADER_APPVERSION:"x-dgw-appversion",HEADER_AUTHTYPE:"x-dgw-authtype",HEADER_AUTHTOKEN:"Authorization",HEADER_DGW_VERSION:"x-dgw-version",HEADER_LOGGING_ID:"x-dgw-loggingid",HEADER_REGIONHINT:"x-dgw-regionhint",HEADER_TARGET_TIER:"x-dgw-tier",HEADER_UUID:"x-dgw-uuid",HEADER_ESTABLISH_STREAM_FRAME_BASE64:"x-dgw-establish-stream-frame-base64",kShadowHeader:"x-dgw-shadow",APPHEADER_PREFIX:"x-dgw-app-"};f.DEFAULT_ACK_TIMEOUT_MS=g;f.DEFAULT_SERVICE_TIER=h;f.MAX_ACK_ID=i;f.WebsocketCloseCodes=j;f.DrainReason=k;f.drainReasonToDrainReasonString=a;f.DgwVersion=l;f.dgwVersionToString=c;f.DgwCodecReturnCode=m;f.DgwCodecReturnCodeToString=d;f.DgwFrameType=n;f.frameTypeToString=e;f.HEADER_CONSTANTS=b}),66);
__d("relay-runtime/mutations/readUpdatableQuery_EXPERIMENTAL",["relay-runtime/query/GraphQLTag","relay-runtime/store/RelayStoreUtils"],(function(a,b,c,d,e,f){"use strict";var g=b("relay-runtime/query/GraphQLTag").getRequest,h=b("relay-runtime/store/RelayStoreUtils").getArgumentValues,i=["id","__id","__typename"];function a(a,b,c){a=g(a);var d={};j(d,c.getRoot(),b,a.fragment.selections,c);return d}function j(a,b,c,d,e){var f=function(){var d;if(o){if(p>=g.length)return"break";q=g[p++]}else{p=g.next();if(p.done)return"break";q=p.value}var f=q;switch(f.kind){case"LinkedField":var r=f.selections.some(function(a){return a.kind==="FragmentSpread"});r=r?f.plural?k(f,c,b,e):l(f,c,b,e):void 0;var s=f.plural?m(f,c,b,e):n(f,c,b,e);Object.defineProperty(a,(d=f.alias)!=null?d:f.name,{get:s,set:r});break;case"ScalarField":s=(d=f.alias)!=null?d:f.name;Object.defineProperty(a,s,{get:function(){var a;a=h((a=f.args)!=null?a:[],c);return b.getValue(f.name,a)},set:i.includes(f.name)?void 0:function(a){var d;d=h((d=f.args)!=null?d:[],c);b.setValue(a,f.name,d)}});break;case"InlineFragment":b.getType()===f.type&&j(a,b,c,f.selections,e);break;case"FragmentSpread":break;default:throw new Error("Encountered an unexpected ReaderSelection variant in RelayRecordSourceProxy. This indicates a bug in Relay.")}};for(var g=d,o=Array.isArray(g),p=0,g=o?g:g[typeof Symbol==="function"?Symbol.iterator:"@@iterator"]();;){var q;d=f();if(d==="break")break}}function k(a,b,c,d){return function(e){var f;f=h((f=a.args)!=null?f:[],b);if(e==null)c.setValue(null,a.name,f);else{e=e.map(function(a){if(a==null)throw new Error("When assigning an array of items, none of the items should be null or undefined.");a=a.__id;if(a==null)throw new Error("The __id field must be present on each item passed to the setter. This indicates a bug in Relay.");var b=d.get(a);if(b==null)throw new Error("Did not find item with data id "+a+" in the store.");return b});c.setLinkedRecords(e,a.name,f)}}}function l(a,b,c,d){return function(e){var f;f=h((f=a.args)!=null?f:[],b);if(e==null)c.setValue(null,a.name,f);else{e=e.__id;if(e==null)throw new Error("The __id field must be present on the argument. This indicates a bug in Relay.");var g=d.get(e);if(g==null)throw new Error("Did not find item with data id "+e+" in the store.");c.setLinkedRecord(g,a.name,f)}}}function m(a,b,c,d){return function(){var e;e=h((e=a.args)!=null?e:[],b);e=c.getLinkedRecords(a.name,e);if(e!=null)return e.map(function(c){if(c!=null){var e={};j(e,c,b,a.selections,d);return e}else return c});else return e}}function n(a,b,c,d){return function(){var e;e=h((e=a.args)!=null?e:[],b);e=c.getLinkedRecord(a.name,e);if(e!=null){var f={};j(f,e,b,a.selections,d);return f}else return e}}e.exports={readUpdatableQuery_EXPERIMENTAL:a}}),null);
__d("RelayFBJsonParser",["cr:267"],(function(a,b,c,d,e,f,g){a={parse:function(a){return b("cr:267")&&h()?b("cr:267")({constructorAction:"preserve",protoAction:"preserve"}).parse(a):JSON.parse(a)}};function h(){return typeof JSON.parse==="function"&&JSON.parse.toString()!=="function parse() { [native code] }"}c=a;g["default"]=c}),98);