#import <Foundation/Foundation.h>

@interface RegionHelper : NSObject
+ (NSString *)getCurrentRegion;
@end

@implementation RegionHelper

+ (NSString *)getCurrentRegion {
    NSLocale *locale = [NSLocale currentLocale];
    NSString *countryCode = [locale objectForKey:NSLocaleCountryCode];
    return countryCode;
}

@end

const char * GetCurrentRegion(void) {
    NSString *region = [RegionHelper getCurrentRegion];
    const char *regionCString = [region UTF8String];
    char *result = (char *)malloc(strlen(regionCString) + 1);
    strcpy(result, regionCString);
    return result;
}

void FreeRegionCString(const char *regionCString) {
    if (regionCString) {
        free((void *)regionCString);
    }
}
